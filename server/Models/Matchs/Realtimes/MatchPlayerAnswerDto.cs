using Google.Cloud.Firestore;

namespace Models.Matchs.Realtimes
{
    [FirestoreData]
    public class MatchPlayerAnswerDto
    {
        [FirestoreProperty]
        public int UserId { get; set; }

        [FirestoreProperty]
        public int QuestionId { get; set; }

        [FirestoreProperty]
        public List<string> Answer { get; set; }

        [FirestoreDocumentCreateTimestamp]
        public DateTime Timestamp { get; set; }
    }
}
