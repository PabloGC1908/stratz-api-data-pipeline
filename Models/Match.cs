using StratzAPI.Models.MatchEvents;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StratzAPI.Models
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        public ICollection<BuildingEvent>? BuildingEvent { get; set; }
        public ICollection<CourierEvent>? CourierEvent { get; set; }
        public ICollection<MatchRuneEvent>? MatchRuneEvent { get; set; }
        public ICollection<RoshanEvent>? RoshanEvent { get; set; }
        public ICollection<TowerDeathEvent>? TowerDeathEvent { get; set; }
        public ICollection<WardEvent>? WardEvent { get; set; }
    }
}
