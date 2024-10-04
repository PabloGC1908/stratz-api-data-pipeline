using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StratzAPI.Models
{
    public class MatchStats
    {
        [Key]
        public long Id { get; set; }
        public long MatchId { get; set; }
        public required Match Match { get; set; }
        public double WinRate { get; set; }
        public double PredictedWinRate { get; set; }
        public int RadiantKills { get; set; }
        public int DireKills { get; set; }
        public int RadiantNetworthLead { get; set; }
        public int RadiantExperienceLead { get; set; }
    }
}
