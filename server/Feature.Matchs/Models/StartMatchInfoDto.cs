namespace Feature.Matchs.Models
{
    public class StartMatchInfoDto
    {
        public string TrackingId { get; set; }
        public List<MatchQuestionContentDto> Questions { get; set; }
        public int MaxSecondPerQuestions { get; set; }
    }
}
