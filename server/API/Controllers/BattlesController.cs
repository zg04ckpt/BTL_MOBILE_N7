using Core.Models;
using Core.Utilities;
using Feature.Matchs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Matchs.Requests;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattlesController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;
        private readonly IMatchService _matchService;

        public BattlesController(
            ILobbyService lobbyService, 
            IMatchService matchService)
        {
            _lobbyService = lobbyService;
            _matchService = matchService;
        }

        [HttpGet("start-options")]
        public async Task<IActionResult> GetStartOptions()
        {
            return Ok(ApiResponse.Success(await _lobbyService.GetOptionsAsync()));
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] SearchMatchRequest request)
        {
            var result = await _matchService.GetAllMatchsPagingAsync(request);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _matchService.GetMatchDetailAsync(id);
            return Ok(ApiResponse.Success(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _matchService.DeleteMatchAsync(id);
            return Ok(ApiResponse.Success("Delete match successfully", result));
        }

        [HttpGet("match-info")]
        [Authorize]
        public async Task<IActionResult> GetMatchInfo()
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchInfoAsync(userId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("match-result")]
        [Authorize]
        public async Task<IActionResult> GetMatchResult()
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchResultAsync(userId);
            return Ok(ApiResponse.Success(result));
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
