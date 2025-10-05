namespace StratzAPI.Models.Events
{
    public class PurchaseEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? ItemId { get; set; }
    }
}
