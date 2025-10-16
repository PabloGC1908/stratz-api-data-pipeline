namespace StratzAPI.DTOs.Match.Events
{
    public class StreakEventDto
    {
        public int Time { get; set; }
        public short? HeroId { get; set; }
        public string? Type { get; set; }
        public int? Value { get; set; }
    }
}
