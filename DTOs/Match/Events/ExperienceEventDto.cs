namespace StratzAPI.DTOs.Match.Events
{
    public class ExperienceEventDto
    {
        public int Time { get; set; }
        public int? Amount { get; set; }
        public string? Reason { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
    }
}
