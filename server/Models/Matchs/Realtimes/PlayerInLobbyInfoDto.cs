using Google.Cloud.Firestore;

namespace Models.Matchs.Realtimes
{
    [FirestoreData]
    public class PlayerInLobbyInfoDto
    {
        [FirestoreProperty]
        public int UserId { get; set; }
        [FirestoreProperty]
        public string AvatarUrl { get; set; }
        [FirestoreProperty]
        public string DisplayName { get; set; }
        [FirestoreProperty]
        public int Level { get; set; }
    }
}
