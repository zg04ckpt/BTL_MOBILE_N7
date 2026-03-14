namespace Models.Events.DTOs.LuckySpin
{
    public class LuckySpinItemDto
    {
        public int ItemId { get; set; }
        public EventRewardDto? Reward { get; set; }
        public double Rate { get; set; }
    }
}
