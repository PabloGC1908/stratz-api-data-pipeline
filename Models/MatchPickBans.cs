using System.ComponentModel.DataAnnotations;

namespace StratzAPI.Models
{
    public class MatchPickBans
    {
        public long Id { get; set; }
        public long? MatchId { get; set; }
        public bool IsPick { get; set; }
        public int HeroId { get; set; }
        public int Order { get; set; }
        public bool IsRadiant { get; set; }
        public int PlayerIndex { get; set; }
        public bool IsCaptain { get; set; }
    }
}
