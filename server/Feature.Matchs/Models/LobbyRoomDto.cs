using Core.Interfaces;
using Feature.Matchs.Enums;
using Feature.Quizzes.Entities;
using Google.Cloud.Firestore;

namespace Feature.Matchs.Models
{
    [FirestoreData]
    public class LobbyRoomDto
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public LobbyRoomStatus Status { get; set; }
        [FirestoreProperty]
        public int TopicId { get; set; }
        [FirestoreProperty]
        public int MaxPlayers { get; set; }
    }
}
