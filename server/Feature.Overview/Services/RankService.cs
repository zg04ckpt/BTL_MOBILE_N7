using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Feature.Overview.Enums;
using Feature.Overview.Interfaces;
using Feature.Overview.Models;
using Feature.Users.Entities;

namespace Feature.Overview.Services
{
    public class RankService : BaseService, IRankService
    {
        public RankService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<UserRankListItemDto[]> GetRankBoardAsync(RankingType rankingType)
        {
            var userRepository = _uow.Repository<User>();

            var users = await userRepository.GetAllAsync(
                predicate: u => true,
                orderBy: u => u.RankScore,
                asc: false
            );

            return users
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
            var userRepository = _uow.Repository<User>();

            var user = await userRepository.GetFirstAsync(
                predicate: u => u.Id == userId
            );

            if (user == null)
                throw new NotFoundException("Người dùng không tồn tại");

            var higherCount = await userRepository.CountAsync(u => u.RankScore > user.RankScore);

            return new UserRankDto
            {
                UserId = user.Id,
                DisplayName = user.Name,
                AvatarUrl = user.AvatarUrl,
                RankScore = user.RankScore,
                Rank = higherCount + 1,
                NumberOfMatchs = 0,
                WinRate = 0
            };
        }
    }
}
