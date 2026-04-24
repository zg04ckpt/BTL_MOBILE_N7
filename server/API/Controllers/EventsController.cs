using Core.Models;
using Feature.Events.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Events.Requests;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSystemEvents()
        {
            var events = await _eventService.GetAllSystemEventsAsync();
            return Ok(ApiResponse.Success(events));
        }

        [HttpGet("ongoing")]
        public async Task<IActionResult> GetOngoingEvents()
        {
            var events = await _eventService.GetOngoingEventsAsync();
            return Ok(ApiResponse.Success(events));
        }

        [HttpGet("rewards/mappings")]
        public async Task<IActionResult> GetEventRewardMappings()
        {
            var mappings = await _eventService.GetEventRewardMappingsAsync();
            return Ok(ApiResponse.Success(mappings));
        }

        [HttpGet("users/my-progress")]
        [Authorize]
        public async Task<IActionResult> GetMyProgress([FromQuery(Name = "eventId")] int eventId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var progress = await _eventService.GetMyProgressByEventAsync(userId, eventId);
            return Ok(ApiResponse.Success(progress));
        }

        [HttpGet("{eventId}/spin")]
        [Authorize]
        public async Task<IActionResult> SpinItem(int eventId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var winItem = await _eventService.SpinItemAsync(userId, eventId);
            return Ok(ApiResponse.Success(new
            {
                WinItem = winItem
            }));
        }

        [HttpPut("my-progress")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProgress([FromBody] UpdateMyEventProgressRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _eventService.UpdateMyProgressAsync(userId, request);
            return Ok(ApiResponse.Success("Event progress updated successfully", result));
        }

        [HttpPatch("my-progresses/quiz-milestone")]
        [Authorize]
        public async Task<IActionResult> UpdateMyQuizMilestoneChallengeProgress([FromBody] UpdateMyQuizMilestoneChallengeProgressRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _eventService.UpdateMyQuizMilestoneChallengeProgressAsync(userId, request);
            return Ok(ApiResponse.Success("Quiz milestone challenge progress updated successfully", result));
        }

        [HttpPatch("claim-reward")]
        [Authorize]
        public async Task<IActionResult> ClaimReward([FromBody] ClaimEventRewardRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _eventService.ClaimRewardAsync(userId, request);
            return Ok(ApiResponse.Success("Reward claimed successfully", result));
        }

        #region Manage

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            var result = await _eventService.CreateEventAsync(request);
            return Ok(ApiResponse.Success("Event created successfully", result));
        }

        [HttpPut("{eventId}")]
        public async Task<IActionResult> Update(int eventId, [FromBody] UpdateEventRequest request)
        {
            var result = await _eventService.UpdateEventAsync(eventId, request);
            return Ok(ApiResponse.Success("Event updated successfully", result));
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> Delete(int eventId)
        {
            var result = await _eventService.DeleteEventAsync(eventId);
            return Ok(ApiResponse.Success("Event deleted successfully", result));
        }

        #endregion
    }
}
