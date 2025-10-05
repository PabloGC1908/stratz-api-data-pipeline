namespace StratzAPI.DTOs.Match.Events
{
    public class PlayerUpdateHealthEventDto
    {
        public int Time { get; set; }
        public int? Hp { get; set; }
        public int? MaxHp { get; set; }
        public int? Mp { get; set; }
        public int? MaxMp { get; set; }
    }
}
