using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Feature.Events.Interfaces;
using Feature.Users.Entities;
using Feature.Users.Utils;
using Microsoft.Extensions.Logging;
using Models.Events.DTOs;
using Models.Events.DTOs.LuckySpin;
using Models.Events.DTOs.QuizMilestoneChallenge;
using Models.Events.DTOs.TournamentRewards;
using Models.Events.Entities;
using Models.Events.Enums;
using Models.Events.Requests;
using Models.Quizzes.Entities;
using System.Text.Json;
using System.Threading.Tasks;

namespace Feature.Events.Services
{
    public class EventService : BaseService, IEventService
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private static readonly Random _rand = new Random();

        public EventService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<ChangedResponse> CreateEventAsync(CreateEventRequest request)
        {
            var config = await ValidateEventConfigAsync(request);

            var @event = new Event
            {
                Name = request.Name,
                IsLocked = false,
                Desc = request.Desc,
                StartTime = request.StartTime?.ToUniversalTime() ?? DateTime.UtcNow,
                EndTime = request.EndTime?.ToUniversalTime(),
                EventTimeType = request.Type == EventType.QuizMilestoneChallenge ?
                    EventTimeType.Limited : request.Type == EventType.LuckySpin ?
                    EventTimeType.Daily : EventTimeType.Seasonal,
                EventConfigJsonData = config,
                EventType = request.Type,
                UserInEvents = new ()
            };

            await _uow.Repository<Event>().AddAsync(@event);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(@event);
        }

        public async Task<ChangedResponse> DeleteEventAsync(int eventId)
        {
            var e = await GetEntityAsync<Event>(eventId);

            await _uow.Repository<Event>().DeleteAsync(e);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(e);
        }

        public async Task<List<object>> GetAllSystemEventsAsync()
        {
            var events = await _uow.Repository<Event>().GetAllAsync(
                predicate: e => true);

            var data = new List<object>();
            foreach (var e in events)
            {
                if (e.EventType == EventType.QuizMilestoneChallenge)
                {
                    data.Add(QuizMilestoneChallengeEventDto.MapFromEntity(e));
                }
                else if (e.EventType == EventType.LuckySpin)
                {
                    data.Add(LuckySpinEventDto.MapFromEntity(e));
                }
            }

            return data;
        }

        public async Task<List<EventRewardInfoDto>> GetEventRewardMappingsAsync()
        {
            var rewards = await _uow.Repository<EventReward>().GetAllAsync(
                predicate: e => true,
                selector: e => new EventRewardInfoDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = e.Type,
                    Desc = e.Desc,
                    Unit = e.Unit
                });

            return rewards.ToList();
        }

        public async Task<List<object>> GetUserInEventProgressesAsync(int userId)
        {
            var data = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == userId,
                selector: e => e.EventsProgress.Select(p => new
                {
                    p.EventId,
                    p.UserId,
                    p.Event.EventType,
                    p.ProgressJsonData,
                    p.LastChanged
                })) ?? throw new NotFoundException("User not found");
            
            var progresses = new List<object>();
            foreach (var item in data)
            {
                if (item.EventType == EventType.QuizMilestoneChallenge)
                {
                    progresses.Add(new QuizMilestoneChallengeProgressDto
                    {
                        EventId = item.EventId,
                        ThresholdProgresses = DeserializeQuizMilestoneProgresses(item.ProgressJsonData),
                        LastChanged = item.LastChanged.ToLocalTime()
                    });
                } 
                else if (item.EventType == EventType.LuckySpin)
                {
                    var spinTime = JsonSerializer.Deserialize<int>(item.ProgressJsonData, JsonSerializerOptions);
                    if (item.LastChanged.Date != DateTime.UtcNow.Date) spinTime = 0;
                    progresses.Add(new LuckySpinProgress
                    {
                        EventId = item.EventId,
                        TodaySpinTime = spinTime,
                        LastChanged = item.LastChanged.ToLocalTime()
                    });
                }
                else if (item.EventType == EventType.TournamentRewards)
                {
                    progresses.Add(new TournamentRewardsProgressDto
                    {
                        EventId = item.EventId,
                        TaskProgresses = JsonSerializer.Deserialize<List<TournamentRewardsProgressInfoDto>>(item.ProgressJsonData, JsonSerializerOptions)!,
                        LastChanged = item.LastChanged.ToLocalTime()
                    });
                }
            }

            return progresses;
        }

        public async Task<object> GetMyProgressByEventAsync(int userId, int eventId)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == userId,
                includes: e => e.EventsProgress)
                ?? throw new NotFoundException("User not found");

            var @event = await GetEntityAsync<Event>(eventId);
            user.EventsProgress ??= new List<UserInEvent>();
            var progress = user.EventsProgress.FirstOrDefault(e => e.EventId == eventId);
            if (progress == null)
            {
                var defaultProgressJson = string.Empty;

                if (@event.EventType == EventType.QuizMilestoneChallenge)
                {
                    var thresholds = JsonSerializer.Deserialize<List<QuizMilestoneChallengeThresholdDto>>(@event.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");

                    var defaultThresholdProgresses = thresholds
                        .Select(t => new QuizMilestoneChallengeProgressInfoDto
                        {
                            ThresholdId = t.ThresholdId,
                            RewardClaimed = false,
                            Completed = false
                        })
                        .ToList();

                    ValidateQuizMilestoneProgresses(defaultThresholdProgresses);
                    defaultProgressJson = JsonSerializer.Serialize(defaultThresholdProgresses, JsonSerializerOptions);
                }
                else if (@event.EventType == EventType.LuckySpin)
                {
                    defaultProgressJson = JsonSerializer.Serialize(0, JsonSerializerOptions);
                }
                else if (@event.EventType == EventType.TournamentRewards)
                {
                    var tasks = JsonSerializer.Deserialize<List<TournamentRewardsTaskDto>>(@event.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");

                    var defaultTaskProgresses = tasks
                        .Select(t => new TournamentRewardsProgressInfoDto
                        {
                            TaskId = t.TaskId,
                            RewardClaimed = false,
                            Completed = false
                        })
                        .ToList();

                    defaultProgressJson = JsonSerializer.Serialize(defaultTaskProgresses, JsonSerializerOptions);
                }
                else
                {
                    throw new BadRequestException("Unsupported event type");
                }

                progress = new UserInEvent
                {
                    UserId = userId,
                    EventId = eventId,
                    ProgressJsonData = defaultProgressJson,
                    LastChanged = DateTime.UtcNow
                };

                user.EventsProgress.Add(progress);
                await _uow.Repository<User>().UpdateAsync(user);
                await _uow.SaveChangesAsync();
            }

            if (@event.EventType == EventType.LuckySpin)
            {
                var spinTime = JsonSerializer.Deserialize<int>(progress.ProgressJsonData, JsonSerializerOptions);
                if (progress.LastChanged.Date != DateTime.UtcNow.Date) spinTime = 0;

                return new LuckySpinProgress
                {
                    EventId = progress.EventId,
                    TodaySpinTime = spinTime,
                    LastChanged = progress.LastChanged.ToLocalTime()
                };
            }

            return MapProgressDto(progress.EventId, @event.EventType, progress.ProgressJsonData, progress.LastChanged);
        }

        public async Task<object> UpdateMyProgressAsync(int userId, UpdateMyEventProgressRequest request)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == userId,
                includes: e => e.EventsProgress)
                ?? throw new NotFoundException("User not found");

            var @event = await GetEntityAsync<Event>(request.EventId);
            var normalizedProgressJson = ValidateAndNormalizeProgressJson(@event.EventType, request.ProgressJsonData);

            user.EventsProgress ??= new List<UserInEvent>();
            var progress = user.EventsProgress.FirstOrDefault(e => e.EventId == request.EventId);

            if (progress == null)
            {
                progress = new UserInEvent
                {
                    UserId = userId,
                    EventId = request.EventId,
                    ProgressJsonData = normalizedProgressJson,
                    LastChanged = DateTime.UtcNow
                };
                user.EventsProgress.Add(progress);
            }
            else
            {
                progress.ProgressJsonData = normalizedProgressJson;
                progress.LastChanged = DateTime.UtcNow;
            }

            await _uow.Repository<User>().UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return MapProgressDto(progress.EventId, @event.EventType, progress.ProgressJsonData, progress.LastChanged);
        }

        public async Task<QuizMilestoneChallengeProgressDto> UpdateMyQuizMilestoneChallengeProgressAsync(int userId, UpdateMyQuizMilestoneChallengeProgressRequest request)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == userId,
                includes: e => e.EventsProgress)
                ?? throw new NotFoundException("User not found");

            var @event = await GetEntityAsync<Event>(request.EventId);
            if (@event.EventType != EventType.QuizMilestoneChallenge)
                throw new BadRequestException("Event must be QuizMilestoneChallenge event");

            user.EventsProgress ??= new List<UserInEvent>();
            var progress = user.EventsProgress.FirstOrDefault(e => e.EventId == request.EventId);

            var thresholdProgresses = progress == null
                ? new List<QuizMilestoneChallengeProgressInfoDto>()
                : DeserializeQuizMilestoneProgresses(progress.ProgressJsonData);

            var thresholdProgress = thresholdProgresses.FirstOrDefault(e => e.ThresholdId == request.ThresholdId);
            if (thresholdProgress == null)
            {
                thresholdProgress = new QuizMilestoneChallengeProgressInfoDto
                {
                    ThresholdId = request.ThresholdId
                };
                thresholdProgresses.Add(thresholdProgress);
            }

            if (request.RewardClaimed.HasValue)
                thresholdProgress.RewardClaimed = request.RewardClaimed.Value;

            if (request.Completed.HasValue)
                thresholdProgress.Completed = request.Completed.Value;

            ValidateQuizMilestoneProgresses(thresholdProgresses);
            var normalizedProgressJson = JsonSerializer.Serialize(thresholdProgresses, JsonSerializerOptions);

            if (progress == null)
            {
                progress = new UserInEvent
                {
                    UserId = userId,
                    EventId = request.EventId,
                    ProgressJsonData = normalizedProgressJson,
                    LastChanged = DateTime.UtcNow
                };
                user.EventsProgress.Add(progress);
            }
            else
            {
                progress.ProgressJsonData = normalizedProgressJson;
                progress.LastChanged = DateTime.UtcNow;
            }

            await _uow.Repository<User>().UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new QuizMilestoneChallengeProgressDto
            {
                EventId = progress.EventId,
                ThresholdProgresses = DeserializeQuizMilestoneProgresses(progress.ProgressJsonData),
                LastChanged = progress.LastChanged.ToLocalTime()
            };
        }

        public async Task<object> ClaimRewardAsync(int userId, ClaimEventRewardRequest request)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == userId,
                includes: e => e.EventsProgress)
                ?? throw new NotFoundException("User not found");

            var @event = await GetEntityAsync<Event>(request.EventId);
            user.EventsProgress ??= new List<UserInEvent>();
            var progress = user.EventsProgress.FirstOrDefault(e => e.EventId == request.EventId);

            if (@event.EventType == EventType.QuizMilestoneChallenge)
            {
                if (!request.ThresholdId.HasValue)
                    throw new BadRequestException("Threshold id is required for QuizMilestoneChallenge event");

                var thresholds = JsonSerializer.Deserialize<List<QuizMilestoneChallengeThresholdDto>>(@event.EventConfigJsonData, JsonSerializerOptions)
                    ?? throw new BadRequestException("Event config JSON has invalid structure");

                var threshold = thresholds.FirstOrDefault(t => t.ThresholdId == request.ThresholdId.Value);

                if (threshold == null)
                    throw new BadRequestException("Threshold does not exist in this event");

                if (progress == null)
                    throw new BadRequestException("Cannot claim reward because this threshold is not completed");

                var thresholdProgresses = DeserializeQuizMilestoneProgresses(progress.ProgressJsonData);
                var thresholdProgress = thresholdProgresses.FirstOrDefault(e => e.ThresholdId == request.ThresholdId.Value)
                    ?? throw new BadRequestException("Threshold progress not found");

                if (!thresholdProgress.Completed)
                    throw new BadRequestException("Cannot claim reward because this threshold is not completed");

                if (thresholdProgress.RewardClaimed)
                    throw new BadRequestException("Reward for this threshold has already been claimed");

                thresholdProgress.RewardClaimed = true;
                ValidateQuizMilestoneProgresses(thresholdProgresses);
                progress.ProgressJsonData = JsonSerializer.Serialize(thresholdProgresses, JsonSerializerOptions);
                progress.LastChanged = DateTime.UtcNow;

                await GrantRewardsToUserAsync(user, userId, threshold.Rewards);
                await _uow.Repository<User>().UpdateAsync(user);
                await _uow.SaveChangesAsync();

                return new QuizMilestoneChallengeProgressDto
                {
                    EventId = progress.EventId,
                    ThresholdProgresses = DeserializeQuizMilestoneProgresses(progress.ProgressJsonData),
                    LastChanged = progress.LastChanged.ToLocalTime()
                };
            }

            if (@event.EventType == EventType.TournamentRewards)
            {
                if (!request.TaskId.HasValue)
                    throw new BadRequestException("Task id is required for TournamentRewards event");

                var tasks = JsonSerializer.Deserialize<List<TournamentRewardsTaskDto>>(@event.EventConfigJsonData, JsonSerializerOptions)
                    ?? throw new BadRequestException("Event config JSON has invalid structure");

                var task = tasks.FirstOrDefault(t => t.TaskId == request.TaskId.Value);

                if (task == null)
                    throw new BadRequestException("Task does not exist in this event");

                if (progress == null)
                    throw new BadRequestException("Cannot claim reward because this task is not completed");

                var taskProgresses = JsonSerializer.Deserialize<List<TournamentRewardsProgressInfoDto>>(progress.ProgressJsonData, JsonSerializerOptions)
                    ?? throw new BadRequestException("Progress JSON has invalid structure");

                var taskProgress = taskProgresses.FirstOrDefault(e => e.TaskId == request.TaskId.Value)
                    ?? throw new BadRequestException("Task progress not found");

                if (!taskProgress.Completed)
                    throw new BadRequestException("Cannot claim reward because this task is not completed");

                if (taskProgress.RewardClaimed)
                    throw new BadRequestException("Reward for this task has already been claimed");

                taskProgress.RewardClaimed = true;
                progress.ProgressJsonData = JsonSerializer.Serialize(taskProgresses, JsonSerializerOptions);
                progress.LastChanged = DateTime.UtcNow;

                await GrantRewardsToUserAsync(user, userId, task.Rewards);
                await _uow.Repository<User>().UpdateAsync(user);
                await _uow.SaveChangesAsync();

                return new TournamentRewardsProgressDto
                {
                    EventId = progress.EventId,
                    TaskProgresses = JsonSerializer.Deserialize<List<TournamentRewardsProgressInfoDto>>(progress.ProgressJsonData, JsonSerializerOptions)!,
                    LastChanged = progress.LastChanged.ToLocalTime()
                };
            }

            throw new BadRequestException("Claim reward only supports QuizMilestoneChallenge and TournamentRewards events");
        }

        public async Task<ChangedResponse> UpdateEventAsync(int eventId, UpdateEventRequest request)
        {
            var config = await ValidateEventConfigAsync(eventId, request);

            var e = await GetEntityAsync<Event>(eventId);
            e.StartTime = request.StartTime.ToUniversalTime();
            e.EndTime = request.EndTime?.ToUniversalTime();
            e.EventConfigJsonData = config;
            e.Desc = request.Desc;
            e.IsLocked = request.IsLocked;
            e.Name = request.Name;

            await _uow.Repository<Event>().UpdateAsync(e); 
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(e);
        }

        public async Task<LuckySpinItemDto> SpinItemAsync(int userId, int eventId)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == userId,
                includes: e => e.EventsProgress); // Lấy lịch sử sự kiện của user

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var spinEvent = await GetEntityAsync<Event>(eventId);
            if (spinEvent.EventType != EventType.LuckySpin)
            {
                throw new BadRequestException("Event must be LuckySpin event");
            }

            var history = user.EventsProgress
                .FirstOrDefault(p => p.EventId == eventId);

            // Nếu lượt quay được cập nhật từ những ngày trước thì reset về 0,
            // Nếu là trong ngày thì lấy ra để kiểm tra số lần quay tối đa
            int spinTime = 0;
            if (history != null && history.LastChanged.Date == DateTime.UtcNow.Date)
            {
                spinTime = JsonSerializer.Deserialize<int>(history.ProgressJsonData, JsonSerializerOptions);
            }

            var spinEventConfig = JsonSerializer.Deserialize<LuckySpinEventDto>(spinEvent.EventConfigJsonData);
            if (spinTime >= spinEventConfig.MaxSpinTimePerDay)
            {
                throw new BadRequestException("The maximum number of spins has been reached");
            }

            // Xử lý quay
            double roll = _rand.NextDouble() * 100;
            double cumulative = 0;
            LuckySpinItemDto? winItem = null;
            foreach (var item in spinEventConfig.SpinItems)
            {
                cumulative += item.Rate;
                if (roll < cumulative)
                {
                    winItem = item;
                    break;
                }
            }
            if (winItem == null)
            {
                throw new ServerErrorException("Failed to spin item, win item is null");
            }

            // Cập nhật lại số lần quay / tạo mới lịch sử nếu là lần đầu quay
            spinTime++;
            if (history == null)
            {
                history = new UserInEvent
                {
                    User = user,
                    Event = spinEvent
                };
                user.EventsProgress.Add(history);
            }
            history.LastChanged = DateTime.UtcNow;
            history.ProgressJsonData = JsonSerializer.Serialize(spinTime);
            await _uow.Repository<User>().UpdateAsync(user);

            // Thêm vật phẩm vào kho của user / thêm user vào danh sách người có vật phẩm này
            if (winItem.Reward != null)
            {
                var reward = await _uow.Repository<EventReward>().GetFirstAsync(
                    predicate: e => e.Id == winItem.Reward.EventRewardId,
                    includes: e => e.ClaimedRewards)
                    ?? throw new ServerErrorException("Reward info is not exists");

                var claimed = reward.ClaimedRewards.FirstOrDefault(cr => cr.UserId == userId);
                if (claimed != null)
                {
                    claimed.Value += winItem.Reward.Value;
                } 
                else
                {
                    reward.ClaimedRewards.Add(new ClaimedReward
                    {
                        User = user,
                        Value = winItem.Reward.Value
                    });
                }

                await _uow.Repository<EventReward>().UpdateAsync(reward);

            }

            // Xử lý nếu quay trúng kinh nghiệm thì cộng thêm kinh nghiệm cho user
            int? expItemId = await _uow.Repository<EventReward>().GetFirstAsync(
                predicate: e => e.Type == EventRewardType.ExpScore,
                selector: e => e.Id);
            if (expItemId != null && expItemId == winItem.Reward.EventRewardId)
            {
                var result = LevelUtil.plusExpScore(user.Level, user.Exp, winItem.Reward.Value);
                user.Level = result.NewLevel;
                user.Exp = result.NewScore;
                await _uow.Repository<User>().UpdateAsync(user);
            }

            await _uow.SaveChangesAsync();

            winItem.Rate = 0;
            return winItem;
        }


        public async Task<List<EventDto>> GetOngoingEventsAsync()
        {
            var events = await _uow.Repository<Event>().GetAllAsync(
                predicate: e => e.IsLocked == false && e.EventType != EventType.TournamentRewards,
                selector: e => new EventDto
                {
                    Id = e.Id,
                    Desc = e.Desc,
                    EndTime = e.EndTime,
                    IsLocked = e.IsLocked,
                    Name = e.Name,
                    StartTime = e.StartTime,
                    TimeType = e.EventTimeType,
                    Type = e.EventType
                }
            );

            return events.ToList();
        }

        #region Private
        private async Task<string> ValidateEventConfigAsync(int eventId, UpdateEventRequest request)
        {
            var existingEvent = await GetEntityAsync<Event>(eventId);
            var eventType = existingEvent.EventType;

            // Check name exists
            var eventRepo = _uow.Repository<Event>();
            var normalizedName = request.Name.Trim().ToLower();
            if (await eventRepo.ExistsAsync(e => e.Id != eventId && e.Name.ToLower() == normalizedName))
                throw new BadRequestException("Event name is already in use");

            try
            {
                if (eventType == EventType.QuizMilestoneChallenge)
                {
                    var thresholds = JsonSerializer.Deserialize<List<QuizMilestoneChallengeThresholdDto>>(request.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");
                    if (thresholds.Count == 0)
                        throw new BadRequestException("Threshold of QuizMilestoneChallenge must have at least 1 elements");

                    foreach (var threshold in thresholds)
                    {
                        if (threshold.ExpScoreGained <= 0)
                            throw new BadRequestException("Value of exp bonus each threshold must have at least 1");
                        if (threshold.ChallengeQuestionIds.Count == 0)
                            throw new BadRequestException("Number of quesitons each threshold must have at least 1");
                        var normalizedQuestionIds = threshold.ChallengeQuestionIds
                            .Where(id => id > 0)
                            .Distinct()
                            .ToList();
                        if (normalizedQuestionIds.Count == 0)
                            throw new BadRequestException("Number of questions each threshold must have at least 1");
                        if (normalizedQuestionIds.Count != await _uow.Repository<Question>().CountAsync(e => normalizedQuestionIds.Contains(e.Id)))
                            throw new BadRequestException("All questions of each threshold must be existing in database");
                        threshold.ChallengeQuestionIds = normalizedQuestionIds;

                        if (threshold.Rewards.Count == 0)
                            throw new BadRequestException("Number of rewards each threshold must have at least 1");
                        foreach (var reward in threshold.Rewards)
                        {
                            ValidateEventRewardDto(reward);
                        }
                    }
                }
                else if (eventType == EventType.LuckySpin)
                {
                    var luckySpinConfig = JsonSerializer.Deserialize<LuckySpinEventDto>(request.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");

                    if (luckySpinConfig.MaxSpinTimePerDay <= 0)
                        throw new BadRequestException("Max spin time per day must be greater than 0");

                    if (luckySpinConfig.SpinItems == null || luckySpinConfig.SpinItems.Count == 0)
                        throw new BadRequestException("LuckySpin must have at least 1 spin item");

                    if (luckySpinConfig.SpinItems.Sum(i => i.Rate) != 100)
                        throw new BadRequestException("Sum of rate must be exactly 100");

                    foreach (var item in luckySpinConfig.SpinItems)
                    {

                        if (item.Rate <= 0)
                            throw new BadRequestException("Rate of each spin item must be greater than 0");

                        if (item.Reward == null)
                            throw new BadRequestException("Each spin item must have a reward");

                        ValidateEventRewardDto(item.Reward);
                    }

                    // Update item id
                    for (int i = 0; i < luckySpinConfig.SpinItems.Count; i++)
                    {
                        luckySpinConfig.SpinItems[i].ItemId = i + 1;
                    }

                    request.EventConfigJsonData = JsonSerializer.Serialize(luckySpinConfig);
                }
                else if (eventType == EventType.TournamentRewards)
                {
                    var tasks = JsonSerializer.Deserialize<List<TournamentRewardsTaskDto>>(request.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");

                    if (tasks.Count == 0)
                        throw new BadRequestException("TournamentRewards must have at least 1 task");

                    foreach (var task in tasks)
                    {
                        if (string.IsNullOrWhiteSpace(task.ShortDesc))
                            throw new BadRequestException("Task short description is required");

                        if (task.NumberOfMatchs <= 0)
                            throw new BadRequestException("Number of matchs in each task must be greater than 0");

                        if (!Enum.IsDefined(typeof(TournamentRewardsTaskType), task.Type))
                            throw new BadRequestException("Task type is invalid");

                        if (task.Rewards == null || task.Rewards.Count == 0)
                            throw new BadRequestException("Number of rewards each task must have at least 1");

                        foreach (var reward in task.Rewards)
                        {
                            ValidateEventRewardDto(reward);
                        }
                    }
                }

                return request.EventConfigJsonData;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw new BadRequestException("Event config JSON has invalid structure");
            }

        }

        private async Task<string> ValidateEventConfigAsync(CreateEventRequest request)
        {
            // Check name exists
            var eventRepo = _uow.Repository<Event>();
            var normalizedName = request.Name.Trim().ToLower();
            if (await eventRepo.ExistsAsync(e => e.Name.ToLower() == normalizedName))
                throw new BadRequestException("Event name is already in use");

            var normalizedConfigJson = request.EventConfigJsonData;
            
            try
            {
                if (request.Type == EventType.QuizMilestoneChallenge)
                {
                    var thresholds = JsonSerializer.Deserialize<List<QuizMilestoneChallengeThresholdDto>>(request.EventConfigJsonData, JsonSerializerOptions) 
                        ?? throw new BadRequestException("Event config JSON has invalid structure");
                    if (thresholds.Count == 0)
                        throw new BadRequestException("Threshold of QuizMilestoneChallenge must have at least 1 elements");

                    int c = 1;
                    foreach (var threshold in thresholds)
                    {
                        threshold.ThresholdId = c++;
                        if (threshold.ExpScoreGained <= 0)
                            throw new BadRequestException("Value of exp bonus each threshold must have at least 1");
                        if (threshold.ChallengeQuestionIds.Count == 0)
                            throw new BadRequestException("Number of quesitons each threshold must have at least 1");
                        var normalizedQuestionIds = threshold.ChallengeQuestionIds
                            .Where(id => id > 0)
                            .Distinct()
                            .ToList();
                        if (normalizedQuestionIds.Count == 0)
                            throw new BadRequestException("Number of questions each threshold must have at least 1");
                        if (normalizedQuestionIds.Count != await _uow.Repository<Question>().CountAsync(e => normalizedQuestionIds.Contains(e.Id)))
                            throw new BadRequestException("All questions of each threshold must be existing in database");
                        threshold.ChallengeQuestionIds = normalizedQuestionIds;

                        if (threshold.Rewards.Count == 0)
                            throw new BadRequestException("Number of rewards each threshold must have at least 1");
                        foreach(var reward in threshold.Rewards)
                        {
                            ValidateEventRewardDto(reward);
                        }
                    }

                    normalizedConfigJson = JsonSerializer.Serialize(thresholds, JsonSerializerOptions);
                }
                else if (request.Type == EventType.LuckySpin)
                {
                    var luckySpinConfig = JsonSerializer.Deserialize<LuckySpinEventDto>(request.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");

                    if (luckySpinConfig.MaxSpinTimePerDay <= 0)
                        throw new BadRequestException("Max spin time per day must be greater than 0");

                    if (luckySpinConfig.SpinItems == null || luckySpinConfig.SpinItems.Count == 0)
                        throw new BadRequestException("LuckySpin must have at least 1 spin item");

                    if (luckySpinConfig.SpinItems.Sum(i => i.Rate) != 100)
                        throw new BadRequestException("Sum of rate must be exactly 100");

                    int c = 1; 
                    foreach (var item in luckySpinConfig.SpinItems)
                    {
                        item.ItemId = c++;

                        if (item.Rate <= 0)
                            throw new BadRequestException("Rate of each spin item must be greater than 0");

                        if (item.Reward == null)
                            throw new BadRequestException("Each spin item must have a reward");

                        ValidateEventRewardDto(item.Reward);
                    }

                    normalizedConfigJson = JsonSerializer.Serialize(luckySpinConfig, JsonSerializerOptions);
                }
                else if (request.Type == EventType.TournamentRewards)
                {
                    var tasks = JsonSerializer.Deserialize<List<TournamentRewardsTaskDto>>(request.EventConfigJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Event config JSON has invalid structure");

                    if (tasks.Count == 0)
                        throw new BadRequestException("TournamentRewards must have at least 1 task");

                    int c = 1;
                    foreach (var task in tasks)
                    {
                        task.TaskId = c++;
                        if (string.IsNullOrWhiteSpace(task.ShortDesc))
                            throw new BadRequestException("Task short description is required");

                        if (task.NumberOfMatchs <= 0)
                            throw new BadRequestException("Number of matchs in each task must be greater than 0");

                        if (!Enum.IsDefined(typeof(TournamentRewardsTaskType), task.Type))
                            throw new BadRequestException("Task type is invalid");

                        if (task.Rewards == null || task.Rewards.Count == 0)
                            throw new BadRequestException("Number of rewards each task must have at least 1");

                        foreach (var reward in task.Rewards)
                        {
                            ValidateEventRewardDto(reward);
                        }
                    }

                    normalizedConfigJson = JsonSerializer.Serialize(tasks, JsonSerializerOptions);
                }
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw new BadRequestException("Event config JSON has invalid structure");
            }

            return normalizedConfigJson;
        }

        private async Task ValidateEventRewardDto(EventRewardDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Reward is required");

            if (!await _uow.Repository<EventReward>().ExistsAsync(e => e.Id == dto.EventRewardId))
                throw new BadRequestException("Event reward not found");

            if (dto.Value <= 0)
                throw new BadRequestException("Reward value must be greater than 0");
        }

        private async Task GrantRewardsToUserAsync(User user, int userId, List<EventRewardDto> rewards)
        {
            if (rewards == null || rewards.Count == 0)
                return;

            var totalExpReward = 0;
            foreach (var rewardDto in rewards)
            {
                var reward = await _uow.Repository<EventReward>().GetFirstAsync(
                    predicate: e => e.Id == rewardDto.EventRewardId,
                    includes: e => e.ClaimedRewards)
                    ?? throw new ServerErrorException("Reward info is not exists");

                reward.ClaimedRewards ??= new List<ClaimedReward>();
                var claimed = reward.ClaimedRewards.FirstOrDefault(cr => cr.UserId == userId);
                if (claimed != null)
                {
                    claimed.Value += rewardDto.Value;
                }
                else
                {
                    reward.ClaimedRewards.Add(new ClaimedReward
                    {
                        User = user,
                        Value = rewardDto.Value
                    });
                }

                if (reward.Type == EventRewardType.ExpScore)
                {
                    totalExpReward += rewardDto.Value;
                }

                await _uow.Repository<EventReward>().UpdateAsync(reward);
            }

            if (totalExpReward > 0)
            {
                var result = LevelUtil.plusExpScore(user.Level, user.Exp, totalExpReward);
                user.Level = result.NewLevel;
                user.Exp = result.NewScore;
            }
        }

        private string ValidateAndNormalizeProgressJson(EventType eventType, string progressJsonData)
        {
            try
            {
                if (eventType == EventType.QuizMilestoneChallenge)
                {
                    var progresses = DeserializeQuizMilestoneProgresses(progressJsonData);
                    ValidateQuizMilestoneProgresses(progresses);
                    return JsonSerializer.Serialize(progresses, JsonSerializerOptions);
                }

                if (eventType == EventType.LuckySpin)
                {
                    var todaySpinTime = JsonSerializer.Deserialize<int>(progressJsonData, JsonSerializerOptions);
                    if (todaySpinTime < 0)
                        throw new BadRequestException("Today spin time must be greater than or equal to 0");

                    return JsonSerializer.Serialize(todaySpinTime, JsonSerializerOptions);
                }

                if (eventType == EventType.TournamentRewards)
                {
                    var taskProgresses = JsonSerializer.Deserialize<List<TournamentRewardsProgressInfoDto>>(progressJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Progress JSON has invalid structure");

                    var duplicatedTaskId = taskProgresses
                        .GroupBy(x => x.TaskId)
                        .FirstOrDefault(x => x.Count() > 1);
                    if (duplicatedTaskId != null)
                        throw new BadRequestException("Task id in progress must be unique");

                    return JsonSerializer.Serialize(taskProgresses, JsonSerializerOptions);
                }

                throw new BadRequestException("Unsupported event type");
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw new BadRequestException("Progress JSON has invalid structure");
            }
        }

        private object MapProgressDto(int eventId, EventType eventType, string progressJsonData, DateTime lastChanged)
        {
            if (eventType == EventType.QuizMilestoneChallenge)
            {
                return new QuizMilestoneChallengeProgressDto
                {
                    EventId = eventId,
                    ThresholdProgresses = DeserializeQuizMilestoneProgresses(progressJsonData),
                    LastChanged = lastChanged.ToLocalTime()
                };
            }

            if (eventType == EventType.LuckySpin)
            {
                return new LuckySpinProgress
                {
                    EventId = eventId,
                    TodaySpinTime = JsonSerializer.Deserialize<int>(progressJsonData, JsonSerializerOptions),
                    LastChanged = lastChanged.ToLocalTime()
                };
            }

            return new TournamentRewardsProgressDto
            {
                EventId = eventId,
                TaskProgresses = JsonSerializer.Deserialize<List<TournamentRewardsProgressInfoDto>>(progressJsonData, JsonSerializerOptions)!,
                LastChanged = lastChanged.ToLocalTime()
            };
        }

        private List<QuizMilestoneChallengeProgressInfoDto> DeserializeQuizMilestoneProgresses(string progressJsonData)
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(progressJsonData);
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<List<QuizMilestoneChallengeProgressInfoDto>>(progressJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Progress JSON has invalid structure");
                }

                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    var singleProgress = JsonSerializer.Deserialize<QuizMilestoneChallengeProgressInfoDto>(progressJsonData, JsonSerializerOptions)
                        ?? throw new BadRequestException("Progress JSON has invalid structure");

                    return new List<QuizMilestoneChallengeProgressInfoDto> { singleProgress };
                }

                throw new BadRequestException("Progress JSON has invalid structure");
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw new BadRequestException("Progress JSON has invalid structure");
            }
        }

        private void ValidateQuizMilestoneProgresses(List<QuizMilestoneChallengeProgressInfoDto> progresses)
        {
            if (progresses == null)
                throw new BadRequestException("Progresses is required");

            foreach (var progress in progresses)
            {
                if (progress.ThresholdId <= 0)
                    throw new BadRequestException("Threshold id must be greater than 0");
            }

            if (progresses.GroupBy(e => e.ThresholdId).Any(g => g.Count() > 1))
                throw new BadRequestException("Threshold id in progress must be unique");
        }

        
        #endregion

    }
}
