namespace StratzAPI.Models.Events
{
    public class ItemUsedEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? ItemId { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
    }
}
