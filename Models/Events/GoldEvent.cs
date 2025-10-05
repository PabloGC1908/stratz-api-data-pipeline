namespace StratzAPI.Models.Events
{
    public class GoldEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? Amount { get; set; }
        public string? Reason { get; set; }
        public int? NpcId { get; set; }
        public bool? IsValidForStats { get; set; }
    }
}
