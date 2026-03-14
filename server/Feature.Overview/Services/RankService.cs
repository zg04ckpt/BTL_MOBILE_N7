using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Feature.Overview.Interfaces;
using Feature.Users.Entities;
using LinqKit;
using Models.Matchs.Entities;
using Models.Matchs.Enums;
using Models.Overviews.DTOs;
using Models.Overviews.Enums;
using Models.Overviews.Requests;

namespace Feature.Overview.Services
{
    public class RankService : BaseService, IRankService
    {
        public RankService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<UserRankListItemDto[]> GetRankBoardAsync(GetRankBoardRequest request)
        {
            // Tạo filter
            var rangeFilter = PredicateBuilder.New<UserMatchResultHistory>(true);
            var now = DateTime.UtcNow;
            DateTime firstDayOfMonth;
            DateTime firstDayOfYear;
            if (request.Type == RankingType.Monthly)
            {
                firstDayOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                rangeFilter = rangeFilter.And(um => um.Match.CreatedAt >= firstDayOfMonth);
                rangeFilter = rangeFilter.And(um => um.Match.CreatedAt <= now);
            }
            else if (request.Type == RankingType.Yearly)
            {
                firstDayOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                rangeFilter = rangeFilter.And(um => um.Match.CreatedAt >= firstDayOfYear);
                rangeFilter = rangeFilter.And(um => um.Match.CreatedAt <= now);
            }

            var users = await _uow.Repository<User>().GetAllAsync(
                predicate: u => true,
                selector: u => new
                {
                    u.Id,
                    u.Name,
                    u.AvatarUrl,
                    RankScore = u.Histories
                        .AsQueryable()
                        .Where(rangeFilter).Sum(h => h.RankScoreGained)
                });

            return users
                .OrderByDescending(u => u.RankScore)
                .Select((u, index) => new UserRankListItemDto
                {
                    UserId = u.Id,
                    DisplayName = u.Name,
                    AvatarUrl = u.AvatarUrl,
                    RankScore = u.RankScore,
                    Rank = index + 1
                })
                .ToArray();
        }

        public async Task<UserRankDto> GetRankInfoAsync(int userId)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: u => u.Id == userId,
                selector: user => new
                {
                    user.Id,
                    user.Name,
                    user.AvatarUrl,
                    Histories = user.Histories
                        .Where(h => h.Match.BattleType == BattleType.Team)
                        .Select(h => new
                        {
                            h.Rank,
                            Timestamp = h.Match.CreatedAt,
                        }).ToList()
                }
            ) ?? throw new NotFoundException("User not found");

            // Calc streak
            int streak = 0, c = 0;
            if (user.Histories.Count > 0)
            {
                user.Histories.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
                for (int i = 0; i < user.Histories.Count; i++)
                {
                    if (user.Histories[i].Rank == 1)
                    {
                        c++;
                        streak = Math.Max(streak, c);
                    }
                    else
                    {
                        c = 0;
                    }
                }
            }

            // Calc rate
            var rate = user.Histories.Count == 0
                ? 0
                : (float)user.Histories.Count(h => h.Rank == 1) / user.Histories.Count * 100f;

            // Calc rank info
            var rankBoard = await GetRankBoardAsync(new GetRankBoardRequest 
            { 
                Type = RankingType.Total 
            });
            var userRankInfo = rankBoard.First(e => e.UserId == userId)!; 

            return new UserRankDto
            {
                UserId = user.Id,
                DisplayName = user.Name,
                AvatarUrl = user.AvatarUrl,
                RankScore = userRankInfo.RankScore,
                Rank = userRankInfo.Rank,
                NumberOfMatchs = user.Histories.Count,
                WinningRate = rate,
                WinningStreak = streak,
            };
        }
    }
}
