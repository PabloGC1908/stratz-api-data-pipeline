namespace StratzAPI.DTOs.Match.Events
{
    public class ItemUsedEventDto
    {
        public int Time { get; set; }
        public short? ItemId { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
    }
}
