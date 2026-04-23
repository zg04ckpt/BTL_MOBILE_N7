namespace Models.Matchs.Requests
{
    public class SubmitMatchAnswerRequest
    {
        public string TrackingId { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public List<string> Answers { get; set; } = new();
    }
}
