using Google.Cloud.Firestore;

namespace Models.Matchs.Realtimes
{
    [FirestoreData]
    public class MatchPlayerInfoDto : PlayerInLobbyInfoDto
    {
        [FirestoreProperty]
        public int Score { get; set; }
        [FirestoreProperty]
        public int Rank { get; set; }
        [FirestoreProperty]
        public int Progress { get; set; }
        [FirestoreProperty]
        public bool IsOnline { get; set; } = true;
        [FirestoreProperty]
        public DateTime LastSeenAtUtc { get; set; } = DateTime.UtcNow;
    }
}
