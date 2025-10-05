namespace StratzAPI.Models.Events
{
    public class BuyBackEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? HeroId { get; set; }
        public int? DeathTimeRemaining { get; set; }
        public int? Cost { get; set; }
    }
}
