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

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
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
                return NotFound(ApiResponse.Failure("Question not found"));
            }

            return Ok(ApiResponse.Success(question));
        }


        [HttpPost]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateQuestionRequest request)
        {
            var result = await _questionService.CreateAsync(request);
            return Ok(ApiResponse.Success("Question created successfully", result));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "OnlyAdmin")]
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
