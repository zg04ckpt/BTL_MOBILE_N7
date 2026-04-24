namespace Models.Events.DTOs.LuckySpin
{
    public class LuckySpinProgress : UserInEventProgressDto
    {
        public int TodaySpinTime { get; set; } // Số lượt đã quay hôm nay
    }
}
