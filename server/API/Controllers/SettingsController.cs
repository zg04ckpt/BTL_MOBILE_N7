using CNLib.Services.Logs;
using Core.Models;
using Feature.Settings.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Settings.Requests;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _configurationService;
        private readonly ILogService<SettingsController> _logService;

        public SettingsController(ISettingsService configurationService, ILogService<SettingsController> logService)
        {
            _configurationService = configurationService;
            _logService = logService;
        }

        [HttpGet]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetSystemConfigurations()
        {
            var configurations = await _configurationService.GetAllSettingsAsync();
            return Ok(ApiResponse.Success(configurations));
        }

        [HttpPut]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> UpdateSystemConfigurations([FromBody] UpdateSettingsRequest request)
        {
            await _configurationService.UpdateSettingsAsync(request);
            return Ok(ApiResponse.Success("System configurations updated successfully"));
        }
    }
}
