namespace StratzAPI.DTOs.Match.Events
{
    public class BuyBackEventDto
    {
        public int Time { get; set; }
        public short? HeroId { get; set; }
        public int? DeathTimeRemaining { get; set; }
        public int? Cost { get; set; }
    }
}
