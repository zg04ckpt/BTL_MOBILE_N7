namespace Models.Quizzes.DTOs
{
    public class TopicListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int QuestionCount { get; set; }
    }
}

