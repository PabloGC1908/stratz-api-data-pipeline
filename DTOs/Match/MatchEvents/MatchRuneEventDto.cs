namespace StratzAPI.DTOs.Match.MatchEvents
{
    public class MatchRuneEventDto
    {
        public int? IndexId { get; set; }
        public int? Time { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public int? Location { get; set; }
        public int? Rune { get; set; }
        public int? Action { get; set; }
    }
}
