namespace Feature.Quiz.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<Question> Questions { get; set; }
    }
}
