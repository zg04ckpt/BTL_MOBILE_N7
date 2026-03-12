using Core.Utilities;
using Feature.Matchs.Interfaces;
using Feature.Matchs.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattlesController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;

        public BattlesController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        [HttpGet("start-options")]
        public async Task<IActionResult> GetStartOptions()
        {
            return Ok(await _lobbyService.GetOptionsAsync());
        }

        [HttpPost("lobbies/join")]
        [Authorize]
        public async Task<IActionResult> JoinLobby([FromBody] JoinLobbyRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            return Ok(new
            {
                LobbyRoomId = await _lobbyService.JoinMatchLobbyAsync(userId, request)
            });
        }

        [HttpPost("lobbies/out")]
        [Authorize]
        public async Task<IActionResult> OutLobby([FromBody] OutLobbyRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            return Ok(new
            {
                LobbyRoomId = await _lobbyService.OutLobbyRoomAsync(userId, request)
            });
        }
    }
}
