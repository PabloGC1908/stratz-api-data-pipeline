namespace StratzAPI.DTOs.Match.Events
{
    public class PlayerUpdatePositionEventDto
    {
        public int Time { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
    }
}
