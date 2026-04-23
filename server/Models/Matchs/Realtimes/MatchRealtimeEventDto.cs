using Google.Cloud.Firestore;

namespace Models.Matchs.Realtimes
{
    public static class MatchRealtimeEventTypes
    {
        public const string LobbyCreated = "lobby_created";
        public const string PlayerJoined = "player_joined";
        public const string PlayerLeft = "player_left";
        public const string MatchFound = "match_found";
        public const string MatchCreated = "match_created";
        public const string AnswerSubmitted = "answer_submitted";
        public const string PlayerFinished = "player_finished";
        public const string LeaderboardUpdated = "leaderboard_updated";
        public const string MatchEnded = "match_ended";
        public const string LoudspeakerUsed = "loudspeaker_used";
    }

    [FirestoreData]
    public class MatchRealtimeEventDto
    {
        [FirestoreProperty]
        public string Type { get; set; } = null!;

        [FirestoreProperty]
        public string Message { get; set; } = null!;

        [FirestoreProperty]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public int? ActorUserId { get; set; }
    }
}
