namespace StratzAPI.DTOs.Match.Events
{
    public class RuneEventDto
    {
        public int Time { get; set; }
        public string? Rune { get; set; }
        public string? Action { get; set; }
        public int? Gold { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
    }
}
