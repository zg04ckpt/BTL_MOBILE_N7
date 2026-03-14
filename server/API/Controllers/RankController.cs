using Core.Models;
using Feature.Overview.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Overviews.Enums;
using Models.Overviews.Requests;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly IRankService _rankService;

        public RankController(IRankService rankService)
        {
            _rankService = rankService;
        }

        [HttpGet("board")]
        public async Task<IActionResult> GetRankBoard([FromQuery] GetRankBoardRequest request)
        {
            var board = await _rankService.GetRankBoardAsync(request);
            return Ok(ApiResponse.Success(board));
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetRankingType()
        {
            return Ok(ApiResponse.Success(new object[]
            {
                nameof(RankingType.Monthly),
                nameof(RankingType.Yearly),
                nameof(RankingType.Total),
            }));
        }

        //[HttpGet("me")]
        //[Authorize]
        //public async Task<IActionResult> GetMyRankInfo()
        //{
        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //    var rankInfo = await _rankService.GetRankInfoAsync(userId);
        //    return Ok(ApiResponse.Success(rankInfo));
        //}

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserRankInfo(int userId)
        {
            var rankInfo = await _rankService.GetRankInfoAsync(userId);
            return Ok(ApiResponse.Success(rankInfo));
        }
    }
}
