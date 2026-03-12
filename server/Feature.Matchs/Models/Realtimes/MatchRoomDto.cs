using Google.Cloud.Firestore;

namespace Feature.Matchs.Models.Realtimes
{
    [FirestoreData]
    public class MatchRoomDto
    {
        [FirestoreProperty]
        public int RoomId { get; set; }

        [FirestoreProperty]
        public List<MatchPlayerInfoDto> Players { get; set; }

        //[FirestoreProperty]
        //public List<MatchQuestionContentDto> Questions { get; set; }

        [FirestoreProperty]
        public List<MatchPlayerAnswerDto> Answers { get; set; }

        [FirestoreProperty]
        public List<string> StatusLogs { get; set; }

        //[FirestoreProperty]
        //public DateTime StartTime { get; set; }

        //[FirestoreProperty]
        //public int MaxTimePerQuestion { get; set; }
    }
}
