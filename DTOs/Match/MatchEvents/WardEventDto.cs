namespace StratzAPI.DTOs.Match.MatchEvents
{
    public class WardEventDto
    {
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
