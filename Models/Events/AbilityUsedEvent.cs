namespace StratzAPI.Models.Events
{
    public class AbilityUsedEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? AbilityId { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
    }
}
