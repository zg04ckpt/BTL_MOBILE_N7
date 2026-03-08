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

        public SystemConfigurationsController(ISystemConfigurationService configurationService)
        {
            _configurationService = configurationService;
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
                return NotFound(ApiResponse.Failure("Configuration not found"));
            }

            return Ok(ApiResponse.Success(config));
        }
    }
}
