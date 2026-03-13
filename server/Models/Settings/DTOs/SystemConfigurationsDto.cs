namespace Models.Settings.DTOs
{
    public class SystemConfigurationsDto
    {
        // General Settings
        public bool MaintenanceMode { get; set; }
        public string WhitelistIPs { get; set; }
        public string RequiredAppVersion { get; set; }
        public int LoginLiveTime { get; set; }
        
        // Game Settings
        public int QuestionTimeLimit { get; set; }
        public int QuestionsPerMatch { get; set; }
        public int BaseWinPoints { get; set; }
        public int BaseLosePoints { get; set; }
        public double EloKFactor { get; set; }
        
        public DateTime LastUpdated { get; set; }
    }
}
