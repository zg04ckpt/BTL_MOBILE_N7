namespace Models.Matchs.DTOs
{
    public class MatchStateDto
    {
        public int MatchId { get; set; }
        public string? TrackingId { get; set; }
        public bool IsEnded { get; set; }
        public int TotalQuestions { get; set; }
        public List<MatchProgressUserDto> Users { get; set; } = new();
    }

    public class MatchProgressUserDto
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Progress { get; set; }
        public int Rank { get; set; }
        public bool IsFinished { get; set; }
    }

    public class MatchReviewDto
    {
        public int MatchId { get; set; }
        public List<UserMatchResultItemDto> Users { get; set; } = new();
        public List<MatchQuestionReviewItemDto> QuestionReviews { get; set; } = new();
    }

    public class MatchQuestionReviewItemDto
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public List<string> CorrectAnswers { get; set; } = new();
        public List<string> YourAnswers { get; set; } = new();
        public bool IsCorrect { get; set; }
    }
}
