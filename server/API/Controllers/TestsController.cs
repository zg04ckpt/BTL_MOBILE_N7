using CNLib.Services.Logs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ILogService<TestsController> _logService;

        public TestsController(ILogService<TestsController> logService)
        {
            _logService = logService;
        }

        [HttpGet] 
        public IActionResult TestRunning()
        {
            _logService.LogInfo("Received tesing request");
            return Ok("API running ...");
        }
    }
}
