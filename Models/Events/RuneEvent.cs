namespace StratzAPI.Models.Events
{
    public class RuneEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public string? Rune { get; set; }
        public string? Action { get; set; }
        public int? Gold { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
    }
}
