using CNLib.Services.Logs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Feature.Matchs.Interfaces;
using Feature.Settings.Interfaces;
using LinqKit;
using Microsoft.Extensions.DependencyInjection;
using Models.Matchs.DTOs;
using Models.Matchs.Entities;
using Models.Matchs.Enums;
using Models.Matchs.Realtimes;
using Models.Matchs.Requests;
using Models.Events.Entities;
using Models.Events.Enums;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Enums;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;

namespace Feature.Matchs.Services
{
    public class MatchService : IMatchService
    {
        private const string FinalizeOnceKey = "__finalize_once__";
        private const int PresenceTimeoutSeconds = 10;
        private const int PresenceCheckIntervalSeconds = 5;
        private static int _isRealtimeAnswerSubscribed;
        private readonly ILogService<MatchService> _logger;
        private readonly IMatchRealtimeService _matchRealtimeService;
        private readonly ISettingsService _settingsService;
        private readonly IUnitOfWork _uow;
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly ConcurrentDictionary<string, MatchRoomMappingDto> _matchs = new();
        private static readonly ConcurrentDictionary<int, MatchReviewDto> _reviewSnapshots = new();
        private static readonly ConcurrentDictionary<int, MatchRoomMappingDto> _endedRoomSnapshots = new();

        public MatchService(
            ILogService<MatchService> logger,
            IUnitOfWork uow,
            IMatchRealtimeService matchRealtimeService,
            ISettingsService settingsService,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _uow = uow;
            _matchRealtimeService = matchRealtimeService;
            _settingsService = settingsService;
            _scopeFactory = scopeFactory;

            if (Interlocked.Exchange(ref _isRealtimeAnswerSubscribed, 1) == 0)
            {
                _matchRealtimeService.OnPlayerPickAnswer += HandlePlayerPickAnswers;
            }
        }

        public async Task<ChangedResponse> DeleteMatchAsync(int matchId)
        {
            var matchToDelete = await _uow.Repository<Match>().GetFirstAsync(
                predicate: e => e.Id == matchId) 
                ?? throw new NotFoundException($"Failed to delete match {matchId}: Not exists");
            await _uow.Repository<Match>().DeleteAsync(matchToDelete);
            await _uow.SaveChangesAsync();
            return new ChangedResponse
            {
                Id = matchId,
            };
        }

        public async Task<Paginated<MatchListItemDto>> GetAllMatchsPagingAsync(SearchMatchRequest request)
        {
            var filterBuilder = PredicateBuilder.New<Match>(true);
            var fromUtc = request.From?.ToUniversalTime();
            var toUtc = request.To?.ToUniversalTime();
            if (request.BattleType != null)
            {
                filterBuilder.And(e => e.BattleType == request.BattleType);
            }
            if (request.NumberOfPlayers != null)
            {
                filterBuilder.And(e => e.NumberOfPlayers == request.NumberOfPlayers);
            }
            if (fromUtc != null)
            {
                filterBuilder.And(e => e.CreatedAt >= fromUtc);
            }
            if (toUtc != null)
            {
                filterBuilder.And(e => e.CreatedAt <= toUtc);
            }

            return await _uow.Repository<Match>().GetPagingAsync(
                predicate: filterBuilder,
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: request.OrderBy ?? nameof(Match.CreatedAt),
                asc: request.IsAsc,
                selector: e => new MatchListItemDto
                {
                    Id = e.Id,
                    From = e.CreatedAt.ToLocalTime(),
                    To = e.EndedAt.ToLocalTime(),
                    BattleStatus = e.Status,
                    BattleType = e.BattleType,
                    ContentType = e.ContentType,
                    NumberOfPlayers = e.NumberOfPlayers,
                    TopicName = e.ContentType == MatchContentType.OnlyOne ? e.Topic!.Name : null
                });
        }

        public async Task<MatchDetailDto> GetMatchDetailAsync(int matchId)
        {
            var match = await _uow.Repository<Match>().GetFirstAsync(
                predicate: e => e.Id == matchId,
                selector: e => new MatchDetailDto
                {
                    Id = e.Id,
                    BattleType = e.BattleType,
                    ContentType = e.ContentType,
                    NumberOfPlayers = e.NumberOfPlayers,
                    CreatedAt = e.CreatedAt.ToLocalTime(),
                    EndedAt = e.EndedAt.ToLocalTime(),
                    MaxSecondPerQuestion = e.MaxSecondPerQuestion,
                    ScorePerCorrectAnswer = e.ScorePerCorrectAnswer,
                    Status = e.Status,
                    TopicId = e.TopicId,
                    TopicName = e.ContentType == MatchContentType.OnlyOne ? e.Topic!.Name : null,
                    Users = e.Users.Select(u => new UserInMatchResultDto
                    {
                        UserId = u.UserId,
                        Status = u.Status,
                        Correct = u.Correct,
                        Duration = u.Duration,
                        Level = u.User.Level,
                        Progress = u.Progress,
                        Score = u.Score,
                        UserDisplayName = u.User.Name
                    }).ToList(),
                    Questions = e.Questions.Select(q => new QuestionListItemDto
                    {
                        Id = q.Question.Id,
                        Level = q.Question.Level,
                        CreatedAt = q.Question.CreatedAt.ToLocalTime(),
                        Status = q.Question.Status,
                        StringContent = q.Question.StringContent,
                        Type = q.Question.Type,
                    }).ToList(),
                }) 
                ?? throw new NotFoundException("Match not found");
            return match;
        }

        public async Task StartMatchAsync(LobbyRoomDto roomInfo, List<PlayerInLobbyInfoDto> players)
        {
            var settings = await _settingsService.GetAllSettingsAsync();
            var questions = await MakeQuestionSetForQuizBattleMatch(
                roomInfo.ContentType,
                roomInfo.TopicId,
                settings.QuestionsPerMatch);
            if (questions.Count < settings.QuestionsPerMatch)
            {
                throw new BadRequestException("Question pool is insufficient for this match configuration");
            }

            // Create new match info
            var match = new Match
            {
                MaxSecondPerQuestion = settings.QuestionTimeLimit,
                CreatedAt = DateTime.UtcNow,
                BattleType = roomInfo.BattleType,
                NumberOfPlayers = roomInfo.MaxPlayers,
                ScorePerCorrectAnswer = 20,
                Status = MatchStatus.InProgress,
                ContentType = roomInfo.ContentType,
                TopicId = roomInfo.ContentType == MatchContentType.OnlyOne ? roomInfo.TopicId : null,
                Questions = questions.Select(q => new QuestionInMatch
                {
                    QuestionId = q.Id,
                    CorrectRate = 0
                }).ToList(),
                Users = players.Select(p => new UserMatchResultHistory
                {
                    UserId = p.UserId,
                    Status = UserInMatchStatus.InMatch,
                    Score = 0,
                    Correct = 0,
                    Duration = 0,
                    Progress = 0
                }).ToList(),
            };

            await _uow.Repository<Match>().AddAsync(match);
            await _uow.SaveChangesAsync();

            // Handle for realtime tracking
            string trackingId;
            try
            {
                trackingId = MapMatchRoomForTracking(questions, players, match, isSolo: false);
                var startRoomResult = await _matchRealtimeService.AddNewMatchRoomAsync(trackingId, players, match);
                if (!startRoomResult)
                {
                    _matchs.TryRemove(trackingId, out _);
                    throw new ServerErrorException("Failed to start new match room");
                }
                _ = WatchMatchTimeoutAsync(trackingId, settings.QuestionTimeLimit * settings.QuestionsPerMatch);
                _ = WatchMatchPresenceTimeoutAsync(trackingId);
            }
            catch (ServerErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"StartMatchAsync: Failed to initialize realtime room - {ex.Message}");
                await _uow.Repository<Match>().DeleteAsync(match);
                await _uow.SaveChangesAsync();
                throw new ServerErrorException("Failed to initialize realtime match room");
            }
        }

        public async Task<StartMatchInfoDto> StartSoloMatchAsync(int userId, StartSoloMatchRequest request)
        {
            if (request.FixedQuestionIds != null && request.FixedQuestionIds.Count > 0)
            {
                request.ContentType = MatchContentType.OnlyOne;
            }

            var user = await _uow.Repository<Feature.Users.Entities.User>().GetFirstAsync(u => u.Id == userId)
                ?? throw new NotFoundException("User not found");

            var settings = await _settingsService.GetAllSettingsAsync();
            List<Question> questions;
            if (request.FixedQuestionIds != null && request.FixedQuestionIds.Count > 0)
            {
                questions = (await _uow.Repository<Question>().GetAllAsync(
                    predicate: q => request.FixedQuestionIds.Contains(q.Id),
                    includes: q => q.Topic)).ToList();

                if (questions.Count != request.FixedQuestionIds.Distinct().Count())
                {
                    throw new BadRequestException("Some fixed question ids are invalid");
                }

                questions = request.FixedQuestionIds
                    .Distinct()
                    .Select(id => questions.First(q => q.Id == id))
                    .ToList();
            }
            else
            {
                questions = await MakeQuestionSetForQuizBattleMatch(
                    request.ContentType,
                    request.TopicId,
                    settings.QuestionsPerMatch);
            }

            var match = new Match
            {
                MaxSecondPerQuestion = settings.QuestionTimeLimit,
                CreatedAt = DateTime.UtcNow,
                BattleType = BattleType.Single,
                NumberOfPlayers = 1,
                ScorePerCorrectAnswer = 20,
                Status = MatchStatus.InProgress,
                ContentType = request.ContentType,
                TopicId = request.ContentType == MatchContentType.OnlyOne ? request.TopicId : null,
                Questions = questions.Select(q => new QuestionInMatch
                {
                    QuestionId = q.Id,
                    CorrectRate = 0
                }).ToList(),
                Users = new List<UserMatchResultHistory>
                {
                    new UserMatchResultHistory
                    {
                        UserId = userId,
                        Status = UserInMatchStatus.InMatch,
                        Score = 0,
                        Correct = 0,
                        Duration = 0,
                        Progress = 0
                    }
                }
            };

            await _uow.Repository<Match>().AddAsync(match);
            await _uow.SaveChangesAsync();

            var trackingId = MapMatchRoomForTracking(
                questions,
                new List<PlayerInLobbyInfoDto>
                {
                    new PlayerInLobbyInfoDto
                    {
                        UserId = user.Id,
                        AvatarUrl = user.AvatarUrl,
                        DisplayName = user.Name,
                        Level = user.Level
                    }
                },
                match,
                isSolo: true);

            _ = WatchMatchTimeoutAsync(trackingId, settings.QuestionTimeLimit * Math.Max(1, questions.Count));
            _ = WatchMatchPresenceTimeoutAsync(trackingId);
            return await GetMatchInfoAsync(userId);
        }

        public async Task<StartMatchInfoDto> GetMatchInfoAsync(int userId)
        {
            KeyValuePair<string, MatchRoomMappingDto> match = default;
            for (var retry = 0; retry < 10; retry++)
            {
                match = _matchs.FirstOrDefault(kvp => kvp.Value.Users.ContainsKey(userId));
                if (!match.Equals(default(KeyValuePair<string, MatchRoomMappingDto>)))
                {
                    break;
                }

                await Task.Delay(250);
            }

            if (match.Equals(default(KeyValuePair<string, MatchRoomMappingDto>)))
            {
                throw new NotFoundException("Match does not exist");
            }
            return await BuildStartMatchInfoAsync(match);
        }

        public async Task<StartMatchInfoDto> GetMatchInfoByTrackingAsync(int userId, string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId))
            {
                throw new BadRequestException("TrackingId is required");
            }

            if (!_matchs.TryGetValue(trackingId, out var room))
            {
                throw new NotFoundException("Match does not exist");
            }

            if (!room.Users.ContainsKey(userId))
            {
                throw new ForbiddenException("User is not in this match");
            }

            var pair = new KeyValuePair<string, MatchRoomMappingDto>(trackingId, room);
            return await BuildStartMatchInfoAsync(pair);
        }

        private async Task<StartMatchInfoDto> BuildStartMatchInfoAsync(KeyValuePair<string, MatchRoomMappingDto> match)
        {
            var questionIds = match.Value.Questions.Values.Select(q => q.QuestionId);
            var questions = await _uow.Repository<Question>().GetAllAsync(
                predicate: e => questionIds.Contains(e.Id),
                includes: e => e.Topic);

            return new StartMatchInfoDto
            {
                MatchId = match.Value.MatchId,
                TrackingId = match.Key,
                IsSolo = match.Value.IsSolo,
                MaxSecondPerQuestions = match.Value.MaxSecondPerQuestion,
                Questions = questions.Select(e => new MatchQuestionContentDto
                {
                    Id = e.Id,
                    AudioUrl = e.AudioUrl,
                    ImageUrl = e.ImageUrl,
                    VideoUrl = e.VideoUrl,
                    Level = e.Level,
                    StringAnswers = JsonSerializer.Deserialize<AnswersDto>(e.AnswerJsonData)!.StringAnswers,
                    CorrectAnswers = match.Value.Questions.TryGetValue(e.Id, out var questionMap)
                        ? questionMap.Answers
                        : new List<string>(),
                    StringContent = e.StringContent,
                    TopicName = e.Topic.Name
                }).ToList(),
            };
        }

        public async Task<MatchStateDto> GetMatchStateAsync(int userId)
        {
            var currentMatch = _matchs.FirstOrDefault(kvp => kvp.Value.Users.ContainsKey(userId));
            if (!currentMatch.Equals(default(KeyValuePair<string, MatchRoomMappingDto>)))
            {
                var room = currentMatch.Value;
                var users = room.Users.Values
                    .OrderByDescending(u => u.Score)
                    .ThenByDescending(u => u.Progress)
                    .ThenBy(u => u.UserId)
                    .Select((u, idx) => new MatchProgressUserDto
                    {
                        UserId = u.UserId,
                        DisplayName = u.DisplayName,
                        AvatarUrl = u.AvatarUrl,
                        Score = u.Score,
                        Correct = u.Answers.Values.Count(a => a.IsCorrect),
                        Progress = u.Progress,
                        Rank = idx + 1,
                        IsFinished = u.IsFinished
                    }).ToList();

                return new MatchStateDto
                {
                    MatchId = room.MatchId,
                    TrackingId = currentMatch.Key,
                    IsEnded = false,
                    TotalQuestions = room.Questions.Count,
                    Users = users
                };
            }

            var endedMatches = await _uow.Repository<Match>().GetAllAsync(
                predicate: m => m.Status == MatchStatus.Ended && m.Users.Any(u => u.UserId == userId),
                orderBy: nameof(Match.EndedAt),
                asc: false,
                pageIndex: 1,
                pageSize: 1);
            var endedMatch = endedMatches.FirstOrDefault();

            if (endedMatch == null)
            {
                throw new NotFoundException("No in-progress or ended match found");
            }

            return new MatchStateDto
            {
                MatchId = endedMatch.Id,
                TrackingId = null,
                IsEnded = true,
                TotalQuestions = 0,
                Users = new()
            };
        }

        public async Task SubmitMatchAnswerAsync(int userId, SubmitMatchAnswerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TrackingId))
            {
                throw new BadRequestException("TrackingId is required");
            }

            if (!_matchs.TryGetValue(request.TrackingId, out var room))
            {
                throw new NotFoundException("Match room not found");
            }

            if (!room.Users.ContainsKey(userId))
            {
                throw new BadRequestException("User is not in this match");
            }

            if (!room.Questions.ContainsKey(request.QuestionId))
            {
                throw new BadRequestException("Question does not belong to this match");
            }

            if (room.IsSolo)
            {
                await ProcessAnswersForRoomAsync(request.TrackingId, room, new List<MatchPlayerAnswerDto>
                {
                    new MatchPlayerAnswerDto
                    {
                        UserId = userId,
                        QuestionId = request.QuestionId,
                        Answer = request.Answers ?? new List<string>()
                    }
                });
            }
            else
            {
                var ok = await _matchRealtimeService.AddPlayerAnswerAsync(request.TrackingId, new MatchPlayerAnswerDto
                {
                    UserId = userId,
                    QuestionId = request.QuestionId,
                    Answer = request.Answers ?? new List<string>()
                });

                if (!ok)
                {
                    throw new ServerErrorException("Failed to submit answer");
                }
            }
        }

        public async Task<MatchLoudspeakerInventoryDto> GetMyLoudspeakerInventoryAsync(int userId)
        {
            var claimed = await _uow.Repository<EventReward>().GetFirstAsync(
                predicate: r => r.Type == EventRewardType.MatchLoudspeaker,
                selector: r => r.ClaimedRewards
                    .Where(cr => cr.UserId == userId)
                    .Select(cr => cr.Value)
                    .FirstOrDefault());

            return new MatchLoudspeakerInventoryDto
            {
                Count = Math.Max(0, claimed)
            };
        }

        public async Task<MatchLoudspeakerInventoryDto> UseLoudspeakerAsync(int userId, UseMatchLoudspeakerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TrackingId))
            {
                throw new BadRequestException("TrackingId is required");
            }

            var normalizedMessage = request.Message?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedMessage))
            {
                throw new BadRequestException("Message is required");
            }

            if (!_matchs.TryGetValue(request.TrackingId, out var room))
            {
                throw new NotFoundException("Match room not found");
            }

            if (!room.Users.ContainsKey(userId))
            {
                throw new BadRequestException("User is not in this match");
            }

            var reward = await _uow.Repository<EventReward>().GetFirstAsync(
                predicate: r => r.Type == EventRewardType.MatchLoudspeaker,
                includes: r => r.ClaimedRewards);

            var claimed = reward?.ClaimedRewards?.FirstOrDefault(cr => cr.UserId == userId);
            if (claimed == null || claimed.Value <= 0)
            {
                throw new BadRequestException("No loudspeaker remaining");
            }

            var appended = await _matchRealtimeService.AppendMatchEventAsync(
                request.TrackingId,
                new MatchRealtimeEventDto
                {
                    Type = MatchRealtimeEventTypes.LoudspeakerUsed,
                    Message = normalizedMessage,
                    ActorUserId = userId
                },
                $"LOUDSPEAKER|{userId}|{normalizedMessage}");
            if (!appended)
            {
                _logger.LogError($"UseLoudspeaker: failed to append realtime event for trackingId {request.TrackingId}, user {userId}");
                throw new ServerErrorException("Không thể phát loa lúc này, vui lòng thử lại");
            }

            claimed.Value--;
            await _uow.Repository<EventReward>().UpdateAsync(reward!);
            await _uow.SaveChangesAsync();

            return new MatchLoudspeakerInventoryDto
            {
                Count = claimed.Value
            };
        }

        private void HandlePlayerPickAnswers(object? sender, (string trackingId, List<MatchPlayerAnswerDto> answers) e)
        {
            _ = HandlePlayerPickAnswersAsync(e.trackingId, e.answers);
        }

        private async Task HandlePlayerPickAnswersAsync(string trackingId, List<MatchPlayerAnswerDto> answers)
        {
            try
            {
                if (!_matchs.TryGetValue(trackingId, out var room))
                {
                    _logger.LogError($"HandlePlayerPickAnswers: tracking room {trackingId} not found");
                    return;
                }
                await ProcessAnswersForRoomAsync(trackingId, room, answers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"HandlePlayerPickAnswers: Unhandled error on match {trackingId} - {ex.Message}");
            }
        }

        private async Task ProcessAnswersForRoomAsync(string trackingId, MatchRoomMappingDto room, List<MatchPlayerAnswerDto> answers)
        {
            var updatedUsers = new List<UserMappingDto>();

            foreach (var answer in answers)
            {
                if (!room.Users.TryGetValue(answer.UserId, out var user))
                    continue;

                if (user.IsFinished)
                    continue;

                if (!room.Questions.TryGetValue(answer.QuestionId, out var question))
                    continue;

                var answerKey = $"{answer.UserId}:{answer.QuestionId}";
                if (!room.ProcessedAnswerKeys.TryAdd(answerKey, true))
                {
                    _logger.LogInfo($"HandlePlayerPickAnswers: duplicate answer ignored {answerKey} in match {trackingId}");
                    continue;
                }

                bool isCorrect = answer.Answer != null
                    && question.Answers != null
                    && answer.Answer.Count == question.Answers.Count
                    && !answer.Answer.Except(question.Answers, StringComparer.OrdinalIgnoreCase).Any();

                user.Answers[answer.QuestionId] = new UserQuestionAnswerMappingDto
                {
                    QuestionId = answer.QuestionId,
                    Answers = answer.Answer ?? new List<string>(),
                    IsCorrect = isCorrect
                };

                if (isCorrect)
                {
                    user.Score += room.ScorePerQuestions;
                }

                user.Progress++;
                if (user.Progress >= room.Questions.Count && !user.IsFinished)
                {
                    user.IsFinished = true;
                    user.FinishedAtUtc = DateTime.UtcNow;
                    if (!room.IsSolo)
                    {
                        _ = _matchRealtimeService.AppendMatchEventAsync(
                            trackingId,
                            new MatchRealtimeEventDto
                            {
                                Type = MatchRealtimeEventTypes.PlayerFinished,
                                Message = $"User {user.UserId} finished all questions"
                            },
                            $"Người chơi {user.UserId} đã hoàn thành toàn bộ câu hỏi");
                    }
                }
                updatedUsers.Add(user);
            }

            if (!room.IsSolo && updatedUsers.Count > 0)
            {
                await _matchRealtimeService.UpdateLeaderboardAsync(trackingId, updatedUsers);
            }

            bool allDone = room.Users.Values.All(u => u.Progress >= room.Questions.Count);
            if (allDone)
            {
                await TryFinalizeMatchAsync(trackingId, room);
            }
        }

        private async Task FinalizeMatchAsync(string trackingId, MatchRoomMappingDto room)
        {
            using var scope = _scopeFactory.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            // Load the Match together with its Users and each user's User profile
            var match = await uow.Repository<Match>().GetFirstAsync(
                predicate: m => m.Id == room.MatchId,
                includes: m => m.Users);

            if (match == null)
            {
                _logger.LogError($"FinalizeMatchAsync: Match {room.MatchId} not found");
                return;
            }

            var finalizedAt = DateTime.UtcNow;

            // Sort by score desc, then duration asc (faster wins tie), then userId for deterministic ordering
            var ranked = room.Users.Values
                .OrderByDescending(u => u.Score)
                .ThenBy(u => u.FinishedAtUtc ?? finalizedAt)
                .ThenBy(u => u.UserId)
                .ToList();

            // Score formulas:
            // ExpGained      : base 50 XP + 10 per correct answer
            // RankScoreGained: winner +BaseWinPoints, others -BaseLosePoints
            var settingsService = scope.ServiceProvider.GetRequiredService<ISettingsService>();
            var settings = await settingsService.GetAllSettingsAsync();
            var rankProtectionReward = await uow.Repository<EventReward>().GetFirstAsync(
                predicate: r => r.Type == EventRewardType.RankProtectionCard,
                includes: r => r.ClaimedRewards);
            int rank = 1;
            foreach (var userMapping in ranked)
            {
                int correctAnswers = userMapping.Score / (room.ScorePerQuestions > 0 ? room.ScorePerQuestions : 1);
                int expGained = userMapping.IsAfk ? 0 : (50 + correctAnswers * 10);
                int rankScoreGained = (match.BattleType == BattleType.Single && match.NumberOfPlayers == 1)
                    ? 0
                    : (rank == 1 ? settings.BaseWinPoints : -settings.BaseLosePoints);
                if (rankScoreGained < 0)
                {
                    var claimedRankProtection = rankProtectionReward?.ClaimedRewards?.FirstOrDefault(cr => cr.UserId == userMapping.UserId);
                    if (claimedRankProtection != null && claimedRankProtection.Value > 0)
                    {
                        claimedRankProtection.Value--;
                        rankScoreGained = 0;
                    }
                }
                var finishedAt = userMapping.FinishedAtUtc ?? finalizedAt;
                var duration = (decimal)(finishedAt - room.MatchStartedAtUtc).TotalSeconds;

                _logger.LogInfo(
                    $"FinalizeMatchAsync [{trackingId}] User {userMapping.UserId}: " +
                    $"rank=#{rank}, score={userMapping.Score}, +{expGained} XP, {rankScoreGained:+#;-#;0} RankScoreGained");

                var history = match.Users.FirstOrDefault(h => h.UserId == userMapping.UserId);
                if (history != null)
                {
                    history.Score = userMapping.Score;
                    history.Progress = userMapping.Progress;
                    history.Correct = correctAnswers;
                    history.Duration = Math.Max(0, duration);
                    history.Rank = rank;
                    history.Status = userMapping.IsFinished
                        ? (userMapping.IsAfk ? UserInMatchStatus.EarlyOut : UserInMatchStatus.Finished)
                        : UserInMatchStatus.EarlyOut;
                    history.ExpScoreGained = expGained;
                    history.RankScoreGained = rankScoreGained;
                }

                rank++;
            }

            match.Status = MatchStatus.Ended;
            match.EndedAt = finalizedAt;
            await uow.Repository<Match>().UpdateAsync(match);
            await uow.SaveChangesAsync();

            _reviewSnapshots[room.MatchId] = new MatchReviewDto
            {
                MatchId = room.MatchId,
                Users = ranked.Select(u => new UserMatchResultItemDto
                {
                    UserId = u.UserId,
                    Name = u.DisplayName,
                    AvatarUrl = u.AvatarUrl,
                    Score = u.Score,
                    ExpGained = match.Users.First(x => x.UserId == u.UserId).ExpScoreGained,
                    RankScoreGained = match.Users.First(x => x.UserId == u.UserId).RankScoreGained,
                    IsRankProtected = match.BattleType != BattleType.Single
                        && match.NumberOfPlayers > 1
                        && match.Users.First(x => x.UserId == u.UserId).Rank > 1
                        && match.Users.First(x => x.UserId == u.UserId).RankScoreGained == 0
                }).ToList(),
                QuestionReviews = new()
            };
            _endedRoomSnapshots[room.MatchId] = room;

            try
            {
                // Always remove Firebase room doc when match is finalized (including solo).
                await _matchRealtimeService.CloseMatchRoomAsync(trackingId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"FinalizeMatchAsync: Failed to close realtime room {trackingId} - {ex.Message}");
            }
            finally
            {
                // Always remove from in-memory tracking
                _matchs.TryRemove(trackingId, out _);
            }
        }

        private async Task TryFinalizeMatchAsync(string trackingId, MatchRoomMappingDto room)
        {
            if (!room.ProcessedAnswerKeys.TryAdd(FinalizeOnceKey, true))
            {
                return;
            }

            await _matchRealtimeService.AppendMatchEventAsync(
                trackingId,
                new MatchRealtimeEventDto
                {
                    Type = MatchRealtimeEventTypes.MatchEnded,
                    Message = "Match ended, finalizing result"
                },
                "Trận đấu đã kết thúc, đang tổng kết kết quả");
            await FinalizeMatchAsync(trackingId, room);
        }

        private async Task WatchMatchTimeoutAsync(string trackingId, int totalMatchSeconds)
        {
            var safeTimeoutSeconds = Math.Max(15, totalMatchSeconds);
            await Task.Delay(TimeSpan.FromSeconds(safeTimeoutSeconds));

            if (!_matchs.TryGetValue(trackingId, out var room))
            {
                return;
            }

            _logger.LogInfo($"WatchMatchTimeoutAsync: timeout reached for match {trackingId}, force finalize");
            await TryFinalizeMatchAsync(trackingId, room);
        }

        private async Task WatchMatchPresenceTimeoutAsync(string trackingId)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(PresenceCheckIntervalSeconds));
                if (!_matchs.TryGetValue(trackingId, out var room))
                {
                    return;
                }

                if (room.IsSolo)
                {
                    continue;
                }

                var now = DateTime.UtcNow;
                // Nguồn sự thật presence lấy từ Firestore do client heartbeat trực tiếp.
                var presenceMap = await _matchRealtimeService.GetPlayerPresenceAsync(trackingId);
                foreach (var kv in presenceMap)
                {
                    if (!room.Users.TryGetValue(kv.Key, out var user)) continue;
                    user.IsOnline = kv.Value.IsOnline;
                    user.LastSeenAtUtc = kv.Value.LastSeenAtUtc;
                }
                var afkAnswers = new List<MatchPlayerAnswerDto>();
                foreach (var user in room.Users.Values.Where(u => !u.IsFinished).ToList())
                {
                    if ((now - user.LastSeenAtUtc).TotalSeconds <= PresenceTimeoutSeconds)
                    {
                        continue;
                    }

                    // User AFK quá timeout: tự động nộp rỗng các câu còn lại.
                    user.IsOnline = false;
                    user.IsAfk = true;
                    await _matchRealtimeService.AppendMatchEventAsync(
                        trackingId,
                        new MatchRealtimeEventDto
                        {
                            Type = MatchRealtimeEventTypes.PlayerFinished,
                            Message = $"User {user.UserId} timeout/AFK, auto submit remaining questions",
                            ActorUserId = user.UserId
                        },
                        $"Người chơi {user.UserId} mất kết nối, hệ thống tự động nộp phần còn lại");

                    foreach (var questionId in room.Questions.Keys)
                    {
                        if (user.Answers.ContainsKey(questionId))
                        {
                            continue;
                        }
                        afkAnswers.Add(new MatchPlayerAnswerDto
                        {
                            UserId = user.UserId,
                            QuestionId = questionId,
                            Answer = new List<string>()
                        });
                    }
                }

                if (afkAnswers.Count > 0)
                {
                    await ProcessAnswersForRoomAsync(trackingId, room, afkAnswers);
                }
            }
        }

        public async Task<MatchResultDto> GetMatchResultAsync(int userId)
        {
            // Find the most recently ended match this user participated in
            var results = await _uow.Repository<Match>().GetAllAsync(
                predicate: m => m.Status == MatchStatus.Ended
                             && m.Users.Any(u => u.UserId == userId),
                orderBy: nameof(Match.EndedAt),
                asc: false,
                pageIndex: 1,
                pageSize: 1,
                selector: m => new MatchResultDto
                {
                    MatchId = m.Id,
                    Users = m.Users
                        .OrderBy(u => u.Rank)
                        .ThenByDescending(u => u.Score)
                        .Select(u => new UserMatchResultItemDto
                        {
                            UserId = u.UserId,
                            Name = u.User.Name,
                            AvatarUrl = u.User.AvatarUrl,
                            Score = u.Score,
                            ExpGained = u.ExpScoreGained,
                            RankScoreGained = u.RankScoreGained,
                            IsRankProtected = m.BattleType != BattleType.Single
                                && m.NumberOfPlayers > 1
                                && u.Rank > 1
                                && u.RankScoreGained == 0
                        }).ToList()
                });

            return results.FirstOrDefault()
                ?? throw new NotFoundException("No completed match result found for this user");
        }

        public async Task<MatchResultDto> GetMatchResultByMatchIdAsync(int userId, int matchId)
        {
            var result = await _uow.Repository<Match>().GetFirstAsync(
                predicate: m => m.Id == matchId
                             && m.Status == MatchStatus.Ended
                             && m.Users.Any(u => u.UserId == userId),
                selector: m => new MatchResultDto
                {
                    MatchId = m.Id,
                    Users = m.Users
                        .OrderBy(u => u.Rank)
                        .ThenByDescending(u => u.Score)
                        .Select(u => new UserMatchResultItemDto
                        {
                            UserId = u.UserId,
                            Name = u.User.Name,
                            AvatarUrl = u.User.AvatarUrl,
                            Score = u.Score,
                            ExpGained = u.ExpScoreGained,
                            RankScoreGained = u.RankScoreGained,
                            IsRankProtected = m.BattleType != BattleType.Single
                                && m.NumberOfPlayers > 1
                                && u.Rank > 1
                                && u.RankScoreGained == 0
                        }).ToList()
                });

            return result ?? throw new NotFoundException($"Match result {matchId} not found for this user");
        }

        public async Task<MatchReviewDto> GetMatchReviewByMatchIdAsync(int userId, int matchId)
        {
            var baseResult = await GetMatchResultByMatchIdAsync(userId, matchId);
            var review = new MatchReviewDto
            {
                MatchId = baseResult.MatchId,
                Users = baseResult.Users
            };

            if (_endedRoomSnapshots.TryGetValue(matchId, out var endedRoom))
            {
                endedRoom.Users.TryGetValue(userId, out var myUser);
                review.QuestionReviews = endedRoom.Questions.Values
                    .Select(q =>
                    {
                        var yourAnswer = myUser?.Answers.TryGetValue(q.QuestionId, out var answer) == true
                            ? answer
                            : null;
                        return new MatchQuestionReviewItemDto
                        {
                            QuestionId = q.QuestionId,
                            QuestionContent = q.QuestionContent,
                            CorrectAnswers = q.Answers,
                            YourAnswers = yourAnswer?.Answers ?? new(),
                            IsCorrect = yourAnswer?.IsCorrect ?? false
                        };
                    }).ToList();
                return review;
            }

            var matchEntity = await _uow.Repository<Match>().GetFirstAsync(
                predicate: m => m.Id == matchId,
                includes: m => m.Questions);
            if (matchEntity == null)
            {
                review.QuestionReviews = new();
                return review;
            }

            var questionIds = matchEntity.Questions.Select(q => q.QuestionId).ToList();
            var questions = await _uow.Repository<Question>().GetAllAsync(
                predicate: q => questionIds.Contains(q.Id));
            review.QuestionReviews = questions.Select(q => new MatchQuestionReviewItemDto
            {
                QuestionId = q.Id,
                QuestionContent = q.StringContent,
                CorrectAnswers = JsonSerializer.Deserialize<AnswersDto>(q.AnswerJsonData)!.CorrectAnswers,
                YourAnswers = new(),
                IsCorrect = false
            }).ToList();
            return review;
        }

        private string MapMatchRoomForTracking(List<Question> questions, List<PlayerInLobbyInfoDto> players, Match match, bool isSolo)
        {
            var trackingId = Guid.NewGuid().ToString();
            _matchs.TryAdd(trackingId, new MatchRoomMappingDto
            {
                MatchId = match.Id,
                IsSolo = isSolo,
                ScorePerQuestions = match.ScorePerCorrectAnswer,
                Questions = questions.ToDictionary(
                    q => q.Id,
                    q => new QuestionMappingDto
                    {
                        QuestionId = q.Id,
                        Answers = JsonSerializer.Deserialize<AnswersDto>(q.AnswerJsonData)!.CorrectAnswers,
                        QuestionContent = q.StringContent
                    }),
                Users = match.Users.ToDictionary(
                    u => u.UserId,
                    u => new UserMappingDto
                    {
                        UserId = u.UserId,
                        DisplayName = players.FirstOrDefault(p => p.UserId == u.UserId)?.DisplayName ?? string.Empty,
                        AvatarUrl = players.FirstOrDefault(p => p.UserId == u.UserId)?.AvatarUrl ?? string.Empty,
                        Progress = u.Progress,
                        Score = u.Score,
                        IsOnline = true,
                        LastSeenAtUtc = DateTime.UtcNow,
                        IsAfk = false,
                        IsFinished = false,
                        FinishedAtUtc = null
                    }),
                MatchStartedAtUtc = match.CreatedAt,
                MaxSecondPerQuestion = match.MaxSecondPerQuestion,
            });

            return trackingId;
        }

        private async Task<List<Question>> MakeQuestionSetForQuizBattleMatch(MatchContentType contentType, int? topicId, int totalQuestions)
        {
            var questionPool = await _uow.Repository<Question>().GetAllAsync(
                predicate: e => true,
                includes: e => e.Topic);

            var safeTotalQuestions = Math.Max(1, totalQuestions);
            var normalCount = (int)Math.Ceiling(safeTotalQuestions * 0.5);
            var mediumCount = (int)Math.Round(safeTotalQuestions * 0.3);
            var hardCount = safeTotalQuestions - normalCount - mediumCount;

            var rng = new Random();

            // Lấy ngẫu nhiên tối đa `count` phần tử từ pool
            List<Question> Pick(IEnumerable<Question> pool, int count)
                => pool.OrderBy(_ => rng.Next())
                       .Take(count)
                       .ToList();

            // Random: hoàn toàn ngẫu nhiên, không phân biệt topic hay level
            if (contentType == MatchContentType.Random)
            {
                var randomSet = Pick(questionPool, safeTotalQuestions);
                if (randomSet.Count < safeTotalQuestions)
                {
                    throw new BadRequestException("Not enough questions to create a random match");
                }
                return randomSet;
            }

            // Mix: hỗn hợp tất cả topic; OnlyOne: lọc đúng topic
            var filtered = contentType == MatchContentType.Mix
                ? questionPool
                : questionPool.Where(q => q.TopicId == topicId).ToList();
            if (filtered.Count() < safeTotalQuestions)
            {
                throw new BadRequestException("Not enough questions for selected topic/content type");
            }

            var byLevel = filtered
                .GroupBy(q => q.Level)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            IEnumerable<Question> PoolOf(QuestionLevel level)
                => byLevel.TryGetValue(level, out var g) ? g : Enumerable.Empty<Question>();

            var selected = new List<Question>();
            selected.AddRange(Pick(PoolOf(QuestionLevel.Normal), normalCount));
            selected.AddRange(Pick(PoolOf(QuestionLevel.Medium), mediumCount));
            selected.AddRange(Pick(PoolOf(QuestionLevel.Hard), hardCount));

            // Bù câu còn thiếu bằng câu bất kỳ chưa được chọn
            if (selected.Count < safeTotalQuestions)
            {
                var fallback = filtered.Where(q => !selected.Contains(q));
                selected.AddRange(Pick(fallback, safeTotalQuestions - selected.Count));
            }

            // Shuffle kết quả cuối
            return selected.OrderBy(_ => rng.Next()).ToList();
        }
    }
}

