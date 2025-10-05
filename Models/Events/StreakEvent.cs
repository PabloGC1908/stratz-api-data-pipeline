namespace StratzAPI.Models.Events
{
    public class StreakEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? HeroId { get; set; }
        public string? Type { get; set; }
        public int? Value { get; set; }
    }
}
