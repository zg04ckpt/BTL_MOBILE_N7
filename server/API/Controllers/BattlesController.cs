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

        [HttpGet("match-info/{trackingId}")]
        [Authorize]
        public async Task<IActionResult> GetMatchInfoByTracking(string trackingId)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchInfoByTrackingAsync(userId, trackingId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("solo/start")]
        [Authorize]
        public async Task<IActionResult> StartSoloMatch([FromBody] StartSoloMatchRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.StartSoloMatchAsync(userId, request);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("match-state")]
        [Authorize]
        public async Task<IActionResult> GetMatchState()
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchStateAsync(userId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("match-answer")]
        [Authorize]
        public async Task<IActionResult> SubmitMatchAnswer([FromBody] SubmitMatchAnswerRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            await _matchService.SubmitMatchAnswerAsync(userId, request);
            return Ok(ApiResponse.Success("Submit answer successfully"));
        }

        [HttpGet("match-tools/loudspeaker")]
        [Authorize]
        public async Task<IActionResult> GetMyLoudspeakerInventory()
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMyLoudspeakerInventoryAsync(userId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("match-tools/loudspeaker/use")]
        [Authorize]
        public async Task<IActionResult> UseLoudspeaker([FromBody] UseMatchLoudspeakerRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.UseLoudspeakerAsync(userId, request);
            return Ok(ApiResponse.Success("Use loudspeaker successfully", result));
        }

        [HttpGet("match-result")]
        [Authorize]
        public async Task<IActionResult> GetMatchResult()
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchResultAsync(userId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("matches/{matchId}/result")]
        [Authorize]
        public async Task<IActionResult> GetMatchResultByMatchId(int matchId)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchResultByMatchIdAsync(userId, matchId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("matches/{matchId}/review")]
        [Authorize]
        public async Task<IActionResult> GetMatchReviewByMatchId(int matchId)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            var result = await _matchService.GetMatchReviewByMatchIdAsync(userId, matchId);
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("lobbies/join")]
        [Authorize]
        public async Task<IActionResult> JoinLobby([FromBody] JoinLobbyRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            return Ok(ApiResponse.Success(new
            {
                LobbyRoomId = await _lobbyService.JoinMatchLobbyAsync(userId, request)
            }));
        }

        [HttpPost("lobbies/out")]
        [Authorize]
        public async Task<IActionResult> OutLobby([FromBody] OutLobbyRequest request)
        {
            var userId = StringUtil.GetUserIdFromClaim(User);
            return Ok(ApiResponse.Success(new
            {
                LobbyRoomId = await _lobbyService.OutLobbyRoomAsync(userId, request)
            }));
        }
    }
}
