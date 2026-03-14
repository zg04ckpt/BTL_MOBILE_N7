namespace Models.Events.DTOs.QuizMilestoneChallenge
{
    public class QuizMilestoneChallengeThresholdDto
    {
        public int ThresholdId { get; set; }
        public int ExpScoreGained { get; set; }
        public List<EventRewardDto> Rewards { get; set; }
        public List<int> ChallengeQuestionIds { get; set; }
    }
}
