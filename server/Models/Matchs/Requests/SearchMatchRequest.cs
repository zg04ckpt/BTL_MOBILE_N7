using Core.Models;
using Models.Matchs.Enums;

namespace Models.Matchs.Requests
{
    public class SearchMatchRequest : PagingRequest
    {
        public BattleType? BattleType { get; set; }
        public int? NumberOfPlayers { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
