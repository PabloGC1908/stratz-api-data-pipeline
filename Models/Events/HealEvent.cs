namespace StratzAPI.Models.Events
{
    public class HealEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
        public int? Value { get; set; }
        public short? ByAbility { get; set; }
        public short? ByItem { get; set; }
    }
}
