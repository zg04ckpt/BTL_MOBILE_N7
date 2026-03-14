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

        [HttpGet("rewards/mappings")]
        public async Task<IActionResult> GetEventRewardMappings()
        {
            var mappings = await _eventService.GetEventRewardMappingsAsync();
            return Ok(ApiResponse.Success(mappings));
        }

        [HttpGet("users/{userId}/progresses")]
        public async Task<IActionResult> GetUserEventProgresses(int userId)
        {
            var progresses = await _eventService.GetUserInEventProgressesAsync(userId);
            return Ok(ApiResponse.Success(progresses));
        }

        [HttpPut("my-progress")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProgress([FromBody] UpdateMyEventProgressRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _eventService.UpdateMyProgressAsync(userId, request);
            return Ok(ApiResponse.Success("Event progress updated successfully", result));
        }

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
    }
}
