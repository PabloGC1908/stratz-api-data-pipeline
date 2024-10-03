using System.ComponentModel.DataAnnotations;

namespace StratzAPI.Models
{
    public class MatchStatsPickBans
    {
        [Key]
        public long Id { get; set; }
        public long MatchId { get; set; }
        public required Match Match { get; set; }
        public bool IsPick {  get; set; }
        public int HeroId { get; set; }
        public int Order {  get; set; }
        public bool isRadiant { get; set; }
        public int playerIndex { get; set; }
        public bool isCaptain { get; set; }
    }
}
