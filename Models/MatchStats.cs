using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StratzAPI.Models
{
    public class MatchStats
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? Match { get; set; }
        public int Min { get; set; }
        public decimal? WinRate { get; set; }
        public decimal? PredictedWinRate { get; set; }
        public int? RadiantKills { get; set; }
        public int? DireKills { get; set; }
        public int? RadiantNetworthLead { get; set; }
        public int? RadiantExperienceLead { get; set; }
    }
}
