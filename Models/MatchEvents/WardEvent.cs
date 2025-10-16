namespace StratzAPI.Models.MatchEvents
{
    public class WardEvent
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? match { get; set; }
        public int? IndexId { get; set; }
        public int? Time { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public int? FromPlayer { get; set; }
        public string? WardType { get; set; }
        public string? Action { get; set; }
        public int? PlayerDestroyed { get; set; }
    }
}
