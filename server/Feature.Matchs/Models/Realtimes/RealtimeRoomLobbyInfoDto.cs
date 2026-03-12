using Google.Cloud.Firestore;

namespace Feature.Matchs.Models.Realtimes
{
    [FirestoreData]
    public class RealtimeRoomLobbyInfoDto
    {
        [FirestoreProperty]
        public string RoomLobbyId { get; set; }
        [FirestoreProperty]
        public int MaxPlayers { get; set; }
        [FirestoreProperty]
        public List<PlayerInLobbyInfoDto> Players { get; set; }
        [FirestoreProperty]
        public List<string> StatusLogs { get; set; }
    }
}
