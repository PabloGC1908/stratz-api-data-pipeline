namespace StratzAPI.DTOs.Match.Events
{
    public class AssistEventDto
    {
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
        public int? Gold { get; set; }
        public int? Xp { get; set; }
        public int? SubTime { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
    }
}
