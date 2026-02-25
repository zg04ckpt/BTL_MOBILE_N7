using Feature.Quiz.Enums;

namespace Feature.Quiz.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string StringContent { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string VideoUrl { get; set; }
        public string AnswerJsonData { get; set; }
        public QuestionType Type { get; set; }
        public QuestionLevel Level { get; set; }
        public QuestionStatus Status { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
