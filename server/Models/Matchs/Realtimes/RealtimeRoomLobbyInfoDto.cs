using Google.Cloud.Firestore;

namespace Models.Matchs.Realtimes
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

        [FirestoreProperty]
        public List<MatchRealtimeEventDto> Events { get; set; } = new();

        [FirestoreProperty]
        public bool StartTriggered { get; set; }
    }
}
