using System.ComponentModel.DataAnnotations;

namespace Models.Settings.Requests
{
    public class UpdateSettingsRequest
    {
        // General Setting
        [Required(ErrorMessage = "Maintenance mode is required")]
        public bool MaintenanceMode { get; set; }

        [MaxLength(1000, ErrorMessage = "Whitelist IPs cannot exceed 1000 characters")]
        public string WhitelistIPs { get; set; } = string.Empty;

        [Required(ErrorMessage = "Login live time is required")]
        [Range(1, 43200, ErrorMessage = "Login live time must be between 1 and 43200 minutes (30 days)")]
        public int LoginLiveTime { get; set; }
        
        // Game Setting
        [Required(ErrorMessage = "Question time limit is required")]
        [Range(5, 300, ErrorMessage = "Time limit must be between 5 and 300 seconds")]
        public int QuestionTimeLimit { get; set; }

        [Required(ErrorMessage = "Questions per match is required")]
        [Range(1, 50, ErrorMessage = "Questions per match must be between 1 and 50")]
        public int QuestionsPerMatch { get; set; }

        [Required(ErrorMessage = "Base win points is required")]
        [Range(0, 1000, ErrorMessage = "Base win points must be between 0 and 1000")]
        public int BaseWinPoints { get; set; }

        [Required(ErrorMessage = "Base lose points is required")]
        [Range(0, 1000, ErrorMessage = "Base lose points must be between 0 and 1000")]
        public int BaseLosePoints { get; set; }

        [Required(ErrorMessage = "ELO K-factor is required")]
        [Range(1, 100, ErrorMessage = "K-factor must be between 1 and 100")]
        public double EloKFactor { get; set; }
    }
}
