using Models.Quizzes.Enums;

namespace Models.Matchs.DTOs
{
    public class MatchQuestionContentDto
    {
        public int Id { get; set; }
        public string StringContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
        public string? VideoUrl { get; set; }
        public QuestionType Type { get; set; }
        public QuestionLevel Level { get; set; }
        public string TopicName { get; set; }
        public List<string> StringAnswers { get; set; }
    }
}
