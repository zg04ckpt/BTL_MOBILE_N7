using CNLib.Services.Logs;
using Core.Models;
using Feature.Overview.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Overviews.Requests;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogService<AnalyticsController> _logService;

        public AnalyticsController(IAnalyticsService analyticsService, ILogService<AnalyticsController> logService)
        {
            _analyticsService = analyticsService;
            _logService = logService;
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
            var (startDate, endDate) = filter.GetDateRange();
            var csvData = await _analyticsService.ExportAnalyticsAsync(filter);
            var fileName = $"analytics_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.csv";
            
            return File(csvData, "text/csv", fileName);
        }
    }
}
