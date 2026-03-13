namespace Models.Quizzes.DTOs
{
    public class TopicDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int QuestionCount { get; set; }
    }
}
