namespace Models.Events.DTOs
{
    public abstract class UserInEventProgressDto
    {
        public int EventId { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
