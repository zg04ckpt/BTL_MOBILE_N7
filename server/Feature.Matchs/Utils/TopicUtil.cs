using Feature.Quizzes.Entities;

namespace Feature.Matchs.Utils
{
    public class TopicUtil
    {
        public static string GetTopicName(int topicId)
        {
            if (topicId == 0) return "Hỗn hợp";
            return "Ngẫu nhiên";
        }
    }
}
