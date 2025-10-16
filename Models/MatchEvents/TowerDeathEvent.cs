namespace StratzAPI.Models.MatchEvents
{
    public class TowerDeathEvent
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? match { get; set; }
        public int? Time { get; set; }
        public int? Radiant { get; set; }
        public int? Dire { get; set; }
    }
}
