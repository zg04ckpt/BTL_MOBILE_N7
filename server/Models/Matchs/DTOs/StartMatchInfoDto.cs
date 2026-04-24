namespace Models.Matchs.DTOs
{
    public class StartMatchInfoDto
    {
        public int MatchId { get; set; }
        public string TrackingId { get; set; }
        public bool IsSolo { get; set; }
        public List<MatchQuestionContentDto> Questions { get; set; }
        public int MaxSecondPerQuestions { get; set; }
    }
}
