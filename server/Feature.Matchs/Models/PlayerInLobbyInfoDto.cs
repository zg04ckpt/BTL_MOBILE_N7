using Google.Cloud.Firestore;

namespace Feature.Matchs.Models
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
