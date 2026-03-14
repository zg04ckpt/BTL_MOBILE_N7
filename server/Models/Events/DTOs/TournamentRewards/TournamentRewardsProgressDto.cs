using Models.Events.DTOs.LuckySpin;
using Models.Events.Entities;
using System.Text.Json;

namespace Models.Events.DTOs.TournamentRewards
{
    public class TournamentRewardsProgressDto : UserInEventProgressDto
    {
        public List<TournamentRewardsProgressInfoDto> TaskProgresses { get; set; }
    }

    public class TournamentRewardsProgressInfoDto
    {
        public int TaskId { get; set; }
        public bool RewardClaimed { get; set; }
        public bool Completed { get; set; }
    }
}
