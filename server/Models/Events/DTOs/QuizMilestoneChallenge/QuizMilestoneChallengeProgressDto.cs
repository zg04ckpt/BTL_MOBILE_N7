using Models.Events.Entities;
using System.Text.Json;

namespace Models.Events.DTOs.QuizMilestoneChallenge
{
    public class QuizMilestoneChallengeProgressDto : UserInEventProgressDto
    {
        public QuizMilestoneChallengeProgressInfoDto Info { get; set; }
    }

    public class QuizMilestoneChallengeProgressInfoDto
    {
        public int CompletedQuestions { get; set; }
        public List<int> RewardClaimedThresholdIds { get; set; }
    }
}
