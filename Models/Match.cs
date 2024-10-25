using System.ComponentModel.DataAnnotations;

namespace StratzAPI.Models
{
    public class Match
    {
        public long Id { get; set; }
        public bool DidRadiantWin { get; set; }
        public int DurationSeconds { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? FirstBloodTime { get; set; }
        public int? RadiantTeamId { get; set; }
        public int? DireTeamId { get; set; }
        public int? GameVersionId { get; set; }

        public ICollection<MatchStats>? MatchStats { get; set; }
        public ICollection<MatchPickBans>? MatchPickBans { get; set; }
        public ICollection<MatchPlayer>? MatchPlayers { get; set; }
    }
}
