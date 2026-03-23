using Core.Models;
using Models.Events.DTOs;
using Models.Events.Requests;

namespace Feature.Events.Interfaces
{
    public interface IEventService
    {
        Task<List<object>> GetAllSystemEventsAsync();
        Task<List<EventRewardInfoDto>> GetEventRewardMappingsAsync();
        Task<ChangedResponse> CreateEventAsync(CreateEventRequest request);
        Task<ChangedResponse> UpdateEventAsync(int eventId, UpdateEventRequest request);
        Task<ChangedResponse> DeleteEventAsync(int eventId);
        Task<List<object>> GetUserInEventProgressesAsync(int userId);
        Task<object> UpdateMyProgressAsync(int userId, UpdateMyEventProgressRequest request);
    }
}
