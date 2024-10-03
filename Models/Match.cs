using System.ComponentModel.DataAnnotations;

namespace StratzAPI.Models
{
    public class Match
    {
        [Key]
        public long Id { get; set; }

        public bool DidRadiantWin { get; set; }
        public int DurationSeconds { get; set; }
        public long StartDateTime { get; set; }
        public long EndDateTime { get; set; }
        public int FirstBloodTime { get; set; }
        public int LeagueId { get; set; }
        public long RadiantTeamId { get; set; }
        public long DireTeamId { get; set; }
        public int GameVersionId { get; set; }

        // Relación 1:N con MatchStatistics
        public ICollection<MatchStatistics>? MatchStatistics { get; set; }
        public ICollection<MatchStatsPickBans>? MatchStatsPickBans { get; set; }
        public ICollection<MatchPlayer>? MatchPlayer { get; set; }
    }
}
