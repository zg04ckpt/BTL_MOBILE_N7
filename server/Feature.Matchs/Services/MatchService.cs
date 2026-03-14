using CNLib.Services.Logs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Feature.Matchs.Interfaces;
using LinqKit;
using Microsoft.Extensions.DependencyInjection;
using Models.Matchs.DTOs;
using Models.Matchs.Entities;
using Models.Matchs.Enums;
using Models.Matchs.Realtimes;
using Models.Matchs.Requests;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Enums;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Feature.Matchs.Services
{
    public class MatchService : IMatchService
    {
        private readonly ILogService<MatchService> _logger;
        private readonly IMatchRealtimeService _matchRealtimeService;
        private readonly IUnitOfWork _uow;
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly ConcurrentDictionary<string, MatchRoomMappingDto> _matchs = new();

        public MatchService(
            ILogService<MatchService> logger,
            IUnitOfWork uow,
            IMatchRealtimeService matchRealtimeService,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _uow = uow;
            _matchRealtimeService = matchRealtimeService;
            _scopeFactory = scopeFactory;
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
            var questions = await MakeQuestionSetForQuizBattleMatch(roomInfo.ContentType, roomInfo.TopicId);

            // Create new match info
            var match = new Match
            {
                MaxSecondPerQuestion = 15,
                CreatedAt = DateTime.UtcNow,
                BattleType = roomInfo.MaxPlayers == 1 ? BattleType.Single : BattleType.Team,
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

            // Handle for realtime tracking
            string trackingId;
            try
            {
                trackingId = MapMatchRoomForTracking(questions, players, match);
                var startRoomResult = await _matchRealtimeService.AddNewMatchRoomAsync(trackingId, players, match);
                if (!startRoomResult)
                {
                    _matchs.TryRemove(trackingId, out _);
                    throw new ServerErrorException("Failed to start new match room");
                }
                _matchRealtimeService.OnPlayerPickAnswer += HandlePlayerPickAnswers;
            }
            catch (ServerErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"StartMatchAsync: Failed to initialize realtime room - {ex.Message}");
                throw new ServerErrorException("Failed to initialize realtime match room");
            }

            await _uow.Repository<Match>().AddAsync(match);
            await _uow.SaveChangesAsync();
        }

        public async Task<StartMatchInfoDto> GetMatchInfoAsync(int userId)
        {
            var match = _matchs.FirstOrDefault(kvp => kvp.Value.Users.ContainsKey(userId));
            if (match.Equals(default(KeyValuePair<string, MatchRoomMappingDto>)))
            {
                throw new NotFoundException("Match does not exist");
            }
            var questionIds = match.Value.Questions.Values.Select(q => q.QuestionId);
            var questions = await _uow.Repository<Question>().GetAllAsync(
                predicate: e => questionIds.Contains(e.Id),
                includes: e => e.Topic);

            return new StartMatchInfoDto
            {
                TrackingId = match.Key,
                MaxSecondPerQuestions = match.Value.MaxSecondPerQuestion,
                Questions = questions.Select(e => new MatchQuestionContentDto
                {
                    Id = e.Id,
                    AudioUrl = e.AudioUrl,
                    ImageUrl = e.ImageUrl,
                    VideoUrl = e.VideoUrl,
                    Level = e.Level,
                    StringAnswers = JsonSerializer.Deserialize<AnswersDto>(e.AnswerJsonData)!.StringAnswers,
                    StringContent = e.StringContent,
                    TopicName = e.Topic.Name
                }).ToList(),
            };
        }

        private async void HandlePlayerPickAnswers(object? sender, (string trackingId, List<MatchPlayerAnswerDto> answers) e)
        {
            try
            {
                if (!_matchs.TryGetValue(e.trackingId, out var room))
                {
                    _logger.LogError($"HandlePlayerPickAnswers: tracking room {e.trackingId} not found");
                    return;
                }

                var updatedUsers = new List<UserMappingDto>();

                foreach (var answer in e.answers)
                {
                    if (!room.Users.TryGetValue(answer.UserId, out var user))
                        continue;

                    if (!room.Questions.TryGetValue(answer.QuestionId, out var question))
                        continue;

                    // Check if the answer is correct (order-insensitive comparison)
                    bool isCorrect = answer.Answer != null
                        && question.Answers != null
                        && answer.Answer.Count == question.Answers.Count
                        && !answer.Answer.Except(question.Answers, StringComparer.OrdinalIgnoreCase).Any();

                    if (isCorrect)
                    {
                        user.Score += room.ScorePerQuestions;
                    }

                    user.Progress++;
                    updatedUsers.Add(user);
                }

                if (updatedUsers.Count > 0)
                {
                    _logger.LogInfo($"HandlePlayerPickAnswers: updating leaderboard for match {e.trackingId}, " +
                        $"{updatedUsers.Count} player(s) answered");
                    await _matchRealtimeService.UpdateLeaderboardAsync(e.trackingId, updatedUsers);
                }

                // Check if all players have completed all questions
                bool allDone = room.Users.Values.All(u => u.Progress >= room.Questions.Count);
                if (allDone)
                {
                    _matchRealtimeService.OnPlayerPickAnswer -= HandlePlayerPickAnswers;
                    await FinalizeMatchAsync(e.trackingId, room);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"HandlePlayerPickAnswers: Unhandled error on match {e.trackingId} - {ex.Message}");
                _matchRealtimeService.OnPlayerPickAnswer -= HandlePlayerPickAnswers;
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

            // Sort in-memory users by score descending to assign ranks
            var ranked = room.Users.Values
                .OrderByDescending(u => u.Score)
                .ToList();

            // Score formulas:
            // ExpGained      : base 50 XP + 10 per correct answer
            // RankScoreGained: 1st +30, 2nd +20, 3rd +10, rest +5; score=0 → -10
            int rank = 1;
            foreach (var userMapping in ranked)
            {
                int correctAnswers = userMapping.Score / (room.ScorePerQuestions > 0 ? room.ScorePerQuestions : 1);
                int expGained = 50 + correctAnswers * 10;
                int rankScoreGained = rank switch
                {
                    1 => 30,
                    2 => 20,
                    3 => 10,
                    _ => 5
                };
                if (userMapping.Score == 0) rankScoreGained = -10;

                _logger.LogInfo(
                    $"FinalizeMatchAsync [{trackingId}] User {userMapping.UserId}: " +
                    $"rank=#{rank}, score={userMapping.Score}, +{expGained} XP, {rankScoreGained:+#;-#;0} RankScoreGained");

                var history = match.Users.FirstOrDefault(h => h.UserId == userMapping.UserId);
                if (history != null)
                {
                    history.Score = userMapping.Score;
                    history.Progress = userMapping.Progress;
                    history.Status = UserInMatchStatus.Finished;
                    history.ExpScoreGained = expGained;
                    history.RankScoreGained = rankScoreGained;
                }

                rank++;
            }

            match.Status = MatchStatus.Ended;
            match.EndedAt = DateTime.UtcNow;
            await uow.Repository<Match>().UpdateAsync(match);
            await uow.SaveChangesAsync();

            try
            {
                // Remove Firebase realtime doc
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
                        .OrderByDescending(u => u.Score)
                        .Select(u => new UserMatchResultItemDto
                        {
                            UserId = u.UserId,
                            Name = u.User.Name,
                            AvatarUrl = u.User.AvatarUrl,
                            Score = u.Score,
                            ExpGained = u.ExpScoreGained,
                            RankScoreGained = u.RankScoreGained
                        }).ToList()
                });

            return results.FirstOrDefault()
                ?? throw new NotFoundException("No completed match result found for this user");
        }

        private string MapMatchRoomForTracking(
            List<Question> questions, 
            List<PlayerInLobbyInfoDto> players, 
            Match match)
        {
            var trackingId = Guid.NewGuid().ToString();
            _matchs.TryAdd(trackingId, new MatchRoomMappingDto
            {
                MatchId = match.Id,
                ScorePerQuestions = match.ScorePerCorrectAnswer,
                Questions = questions.ToDictionary(
                    q => q.Id,
                    q => new QuestionMappingDto
                    {
                        QuestionId = q.Id,
                        Answers = JsonSerializer.Deserialize<AnswersDto>(q.AnswerJsonData)!.CorrectAnswers
                    }),
                Users = match.Users.ToDictionary(
                    u => u.UserId,
                    u => new UserMappingDto
                    {
                        UserId = u.UserId,
                        Progress = u.Progress,
                        Score = u.Score
                    }),
                MaxSecondPerQuestion = match.MaxSecondPerQuestion,
            });

            return trackingId;
        }

        /// <summary>
        /// Sinh danh sách câu hỏi cho trận đấu.
        /// Random  : hoàn toàn ngẫu nhiên, không phân biệt topic hay level
        /// Mix     : hỗn hợp nhiều topic, phân tỉ lệ level
        /// OnlyOne : lấy đúng topic chỉ định, phân tỉ lệ level
        /// </summary>
        private async Task<List<Question>> MakeQuestionSetForQuizBattleMatch(MatchContentType contentType, int? topicId)
        {
            var questionPool = await _uow.Repository<Question>().GetAllAsync(
                predicate: e => true,
                includes: e => e.Topic);

            const int TotalQuestions = 2;
            const int NormalCount   = 5;   // 50%
            const int MediumCount   = 3;   // 30%
            const int HardCount     = 2;   // 20%

            var rng = new Random();

            // Lấy ngẫu nhiên tối đa `count` phần tử từ pool
            List<Question> Pick(IEnumerable<Question> pool, int count)
                => pool.OrderBy(_ => rng.Next())
                       .Take(count)
                       .ToList();

            // Random: hoàn toàn ngẫu nhiên, không phân biệt topic hay level
            if (contentType == MatchContentType.Random)
                return Pick(questionPool, TotalQuestions);

            // Mix: hỗn hợp tất cả topic; OnlyOne: lọc đúng topic
            var filtered = contentType == MatchContentType.Mix
                ? questionPool
                : questionPool.Where(q => q.TopicId == topicId).ToList();

            var byLevel = filtered
                .GroupBy(q => q.Level)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            IEnumerable<Question> PoolOf(QuestionLevel level)
                => byLevel.TryGetValue(level, out var g) ? g : Enumerable.Empty<Question>();

            var selected = new List<Question>();
            selected.AddRange(Pick(PoolOf(QuestionLevel.Normal), NormalCount));
            selected.AddRange(Pick(PoolOf(QuestionLevel.Medium), MediumCount));
            selected.AddRange(Pick(PoolOf(QuestionLevel.Hard),   HardCount));

            // Bù câu còn thiếu bằng câu bất kỳ chưa được chọn
            if (selected.Count < TotalQuestions)
            {
                var fallback = filtered.Where(q => !selected.Contains(q));
                selected.AddRange(Pick(fallback, TotalQuestions - selected.Count));
            }

            // Shuffle kết quả cuối
            return selected.OrderBy(_ => rng.Next()).ToList();
        }
    }
}

