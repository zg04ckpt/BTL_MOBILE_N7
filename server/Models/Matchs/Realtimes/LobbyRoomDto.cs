using Core.Interfaces;
using Models.Matchs.Enums;
using Models.Quizzes.Entities;
using Google.Cloud.Firestore;

namespace Models.Matchs.Realtimes
{
    [FirestoreData]
    public class LobbyRoomDto
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public LobbyRoomStatus Status { get; set; }
        [FirestoreProperty]
        public MatchContentType ContentType { get; set; }
        [FirestoreProperty]
        public BattleType BattleType { get; set; }
        [FirestoreProperty]
        public int? TopicId { get; set; }
        [FirestoreProperty]
        public int MaxPlayers { get; set; }
    }
}
