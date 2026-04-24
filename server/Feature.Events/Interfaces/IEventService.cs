using Core.Models;
using Models.Events.DTOs;
using Models.Events.DTOs.LuckySpin;
using Models.Events.DTOs.QuizMilestoneChallenge;
using Models.Events.Requests;

namespace Feature.Events.Interfaces
{
    public interface IEventService
    {
        Task<List<object>> GetAllSystemEventsAsync();
        Task<List<EventDto>> GetOngoingEventsAsync();
        Task<List<EventRewardInfoDto>> GetEventRewardMappingsAsync();
        Task<ChangedResponse> CreateEventAsync(CreateEventRequest request);
        Task<ChangedResponse> UpdateEventAsync(int eventId, UpdateEventRequest request);
        Task<ChangedResponse> DeleteEventAsync(int eventId);
        Task<List<object>> GetUserInEventProgressesAsync(int userId);
        Task<object> GetMyProgressByEventAsync(int userId, int eventId);
        Task<object> UpdateMyProgressAsync(int userId, UpdateMyEventProgressRequest request);
        Task<QuizMilestoneChallengeProgressDto> UpdateMyQuizMilestoneChallengeProgressAsync(int userId, UpdateMyQuizMilestoneChallengeProgressRequest request);
        Task<object> ClaimRewardAsync(int userId, ClaimEventRewardRequest request);
        Task<LuckySpinItemDto> SpinItemAsync(int userId, int eventId);
    }
}
