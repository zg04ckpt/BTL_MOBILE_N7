using CNLib.Services.Logs;
using Core.Models;
using Feature.Quizzes.Interfaces;
using Feature.Quizzes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly ILogService<TopicsController> _logService;

        public TopicsController(ITopicService topicService, ILogService<TopicsController> logService)
        {
            _topicService = topicService;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var topics = await _topicService.GetAllAsync();
            return Ok(ApiResponse.Success(topics));
        }


        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] SearchTopicRequest request)
        {
            var result = await _topicService.GetPagingAsync(request);
            return Ok(ApiResponse.Success(result));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var topic = await _topicService.GetByIdAsync(id);
            
            if (topic == null)
            {
                _logService.LogError($"[API] Topic not found: #{id}");
                return NotFound(ApiResponse.Failure("Topic not found"));
            }

            return Ok(ApiResponse.Success(topic));
        }


        [HttpPost]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateTopicRequest request)
        {
            var result = await _topicService.CreateAsync(request);
            return Ok(ApiResponse.Success("Topic created successfully", result));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTopicRequest request)
        {
            var result = await _topicService.UpdateAsync(id, request);
            return Ok(ApiResponse.Success("Topic updated successfully", result));
        }

        /// <summary>
        /// Delete topic (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _topicService.DeleteAsync(id);
            return Ok(ApiResponse.Success("Topic deleted successfully", result));
        }
    }
}
