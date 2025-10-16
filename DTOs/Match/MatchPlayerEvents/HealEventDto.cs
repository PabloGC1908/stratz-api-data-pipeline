namespace StratzAPI.DTOs.Match.Events
{
    public class HealEventDto
    {
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
        public int? Value { get; set; }
        public short? ByAbility { get; set; }
        public short? ByItem { get; set; }
    }
}
