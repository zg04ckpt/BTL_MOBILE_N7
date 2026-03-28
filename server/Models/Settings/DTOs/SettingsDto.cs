namespace Models.Settings.DTOs
{
    public class SettingsDto
    {
        // General Setting
        public bool MaintenanceMode { get; set; }
        public string WhitelistIPs { get; set; }
        public int LoginLiveTime { get; set; }
        
        // Game Setting
        public int QuestionTimeLimit { get; set; }
        public int QuestionsPerMatch { get; set; }
        public int BaseWinPoints { get; set; }
        public int BaseLosePoints { get; set; }
        public double EloKFactor { get; set; }
        
        public DateTime LastUpdated { get; set; }
    }
}
