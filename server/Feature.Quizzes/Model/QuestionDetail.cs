using Feature.Quiz.Entities;
using Feature.Quiz.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Feature.Quiz.Model
{
    public class QuestionDetail
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string StringContent { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string VideoUrl { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionLevel Level { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionStatus Status { get; set; }
        public int TopicId { get; set; }
        public List<string> CorrectAnswers { get; set; }
        public List<string> StringAnswers { get; set; }

        public QuestionDetail(Question entity)
        {
            Id = entity.Id;
            Slug = entity.Slug;
            StringContent = entity.StringContent;
            ImageUrl = entity.ImageUrl;
            AudioUrl = entity.AudioUrl;
            VideoUrl = entity.VideoUrl;
            Type = entity.Type;
            Level = entity.Level;
            Status = entity.Status;
            TopicId = entity.TopicId;
            ConvertFromJsonToAnswers(entity.AnswerJsonData);
        }

        private void ConvertFromJsonToAnswers(string answerJsonData)
        {
            var answers = JsonSerializer.Deserialize<Answers>(answerJsonData);
            CorrectAnswers = answers.CorrectAnswers;
            StringAnswers = answers.StringAnswers;
        }
    }
}
