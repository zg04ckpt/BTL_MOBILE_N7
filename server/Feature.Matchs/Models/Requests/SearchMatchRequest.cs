using Core.Models;
using Feature.Matchs.Enums;

namespace Feature.Matchs.Models.Requests
{
    public class SearchMatchRequest : PagingRequest
    {
        public BattleType? BattleType { get; set; }
        public int? NumberOfPlayers { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
