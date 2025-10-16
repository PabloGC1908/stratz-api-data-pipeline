namespace StratzAPI.DTOs.Match.Events
{
    public class AbilityUsedEventDto
    {
        public int Time { get; set; }
        public short? AbilityId { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
    }
}
