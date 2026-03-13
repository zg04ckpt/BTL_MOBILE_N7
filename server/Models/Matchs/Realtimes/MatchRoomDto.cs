using Google.Cloud.Firestore;
using Models.Matchs.Realtimes;

namespace Models.Matchs.Realtimes
{
    [FirestoreData]
    public class MatchRoomDto
    {
        [FirestoreProperty]
        public int RoomId { get; set; }

        [FirestoreProperty]
        public List<MatchPlayerInfoDto> Players { get; set; }

        [FirestoreProperty]
        public List<MatchPlayerAnswerDto> Answers { get; set; }

        [FirestoreProperty]
        public List<string> StatusLogs { get; set; }
    }
}
