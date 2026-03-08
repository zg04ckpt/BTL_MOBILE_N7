using Core.Models;
using Feature.Overview.Interfaces;
using Feature.Overview.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetSystemAnalytics([FromQuery] AnalyticsFilterRequest filter)
        {
            var analytics = await _analyticsService.GetSystemAnalyticsAsync(filter);
            return Ok(ApiResponse.Success(analytics));
        }


        [HttpGet("recent-users")]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetRecentUsers([FromQuery] RecentUsersRequest request)
        {
            var users = await _analyticsService.GetRecentUsersAsync(request);
            return Ok(ApiResponse.Success(users));
        }


        [HttpGet("export")]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> ExportAnalytics([FromQuery] AnalyticsFilterRequest filter)
        {
            var csvData = await _analyticsService.ExportAnalyticsAsync(filter);
            var (startDate, endDate) = filter.GetDateRange();
            var fileName = $"analytics_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.csv";
            
            return File(csvData, "text/csv", fileName);
        }
    }
}
