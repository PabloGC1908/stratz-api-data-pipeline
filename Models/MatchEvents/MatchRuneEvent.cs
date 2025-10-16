namespace StratzAPI.Models.MatchEvents
{
    public class MatchRuneEvent
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? match { get; set; }
        public int? IndexId { get; set; }
        public int? Time { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public int? Location { get; set; }
        public int? Rune { get; set; }
        public int? Action { get; set; }
    }
}
