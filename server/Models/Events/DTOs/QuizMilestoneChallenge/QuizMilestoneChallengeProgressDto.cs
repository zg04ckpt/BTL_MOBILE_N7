namespace Models.Events.DTOs.QuizMilestoneChallenge
{
    public class QuizMilestoneChallengeProgressDto : UserInEventProgressDto
    {
        public List<QuizMilestoneChallengeProgressInfoDto> ThresholdProgresses { get; set; } = new();
    }

    public class QuizMilestoneChallengeProgressInfoDto
    {
        public int ThresholdId { get; set; }
        public bool RewardClaimed { get; set; }
        public bool Completed { get; set; }
    }
}
