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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ILogService<QuestionsController> _logService;

        public QuestionsController(IQuestionService questionService, ILogService<QuestionsController> logService)
        {
            _questionService = questionService;
            _logService = logService;
        }


        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] SearchQuestionRequest request)
        {
            var result = await _questionService.GetPagingAsync(request);
            return Ok(ApiResponse.Success(result));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _questionService.GetByIdAsync(id);
            
            if (question == null)
            {
                _logService.LogError($"[API] Question not found: #{id}");
                return NotFound(ApiResponse.Failure("Question not found"));
            }

            return Ok(ApiResponse.Success(question));
        }


        [HttpPost]
        [Authorize(Policy = "OnlyAdmin")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
        public async Task<IActionResult> Create([FromForm] CreateQuestionRequest request)
        {
            var result = await _questionService.CreateAsync(request);
            return Ok(ApiResponse.Success("Question created successfully", result));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "OnlyAdmin")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateQuestionRequest request)
        {
            var result = await _questionService.UpdateAsync(id, request);
            return Ok(ApiResponse.Success("Question updated successfully", result));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _questionService.DeleteAsync(id);
            return Ok(ApiResponse.Success("Question deleted successfully", result));
        }
    }
}
