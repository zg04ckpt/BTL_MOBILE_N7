using CNLib.Services.Logs;
using Core.Models;
using Feature.Settings.Interfaces;
using Feature.Settings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemConfigurationsController : ControllerBase
    {
        private readonly ISystemConfigurationService _configurationService;
        private readonly ILogService<SystemConfigurationsController> _logService;

        public SystemConfigurationsController(ISystemConfigurationService configurationService, ILogService<SystemConfigurationsController> logService)
        {
            _configurationService = configurationService;
            _logService = logService;
        }

        [HttpGet]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetSystemConfigurations()
        {
            var configurations = await _configurationService.GetSystemConfigurationsAsync();
            return Ok(ApiResponse.Success(configurations));
        }

        [HttpPut]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> UpdateSystemConfigurations([FromBody] UpdateSystemConfigurationsRequest request)
        {
            await _configurationService.UpdateSystemConfigurationsAsync(request);
            return Ok(ApiResponse.Success("System configurations updated successfully"));
        }

        [HttpGet("{key}")]
        //[Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var config = await _configurationService.GetByKeyAsync(key);
            
            if (config == null)
            {
                _logService.LogError($"[API] Config not found: {key}");
                return NotFound(ApiResponse.Failure("Configuration not found"));
            }

            return Ok(ApiResponse.Success(config));
        }
    }
}
