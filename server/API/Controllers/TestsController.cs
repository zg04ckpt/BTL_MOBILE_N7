using CNLib.Services.Logs;
using Feature.Matchs.Enums;
using Feature.Matchs.Interfaces;
using Feature.Matchs.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ILogService<TestsController> _logService;
        private readonly IMatchRealtimeService _matchRealtimeService;

        public TestsController(
            ILogService<TestsController> logService, 
            IMatchRealtimeService matchRealtimeService)
        {
            _logService = logService;
            _matchRealtimeService = matchRealtimeService;
        }

        [HttpGet] 
        public IActionResult TestRunning()
        {
            _logService.LogInfo("Received tesing request");
            return Ok("API running ...");
        }

        [HttpGet("realtime/create-test-lobby-room")]
        public async Task<IActionResult> TestCreateLobbyRoom()
        {
            return Ok(await _matchRealtimeService.AddNewLobbyRoomAsync(new LobbyRoomDto
            {
                Id = "test",
                MaxPlayers = 5,
                Status = LobbyRoomStatus.InQueue,
                TopicId = -1
            }));
        }

        [HttpGet("realtime/add-test-player-to-test-lobby-room")]
        public async Task<IActionResult> TestAddPlayerToLobbyRoom()
        {
            return Ok(await _matchRealtimeService.AddPlayerToRoomAsync("test", new PlayerInLobbyInfoDto
            {
                AvatarUrl = "",
                DisplayName = "HCN",
                Level = 1,
                UserId = 1
            }));
        }
    }
}
