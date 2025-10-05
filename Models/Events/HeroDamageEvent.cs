namespace StratzAPI.Models.Events
{
    public class HeroDamageEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public short? Target { get; set; }
        public int? Value { get; set; }
        public int? ByAbility { get; set; }
        public int? ByItem { get; set; }
        public string? DamageType { get; set; }
        public short? FromNpc { get; set; }
        public short? ToNpc { get; set; }
        public bool? FromIllusion { get; set; }
        public bool? ToIllusion { get; set; }
        public bool? IsPhysicalAttack { get; set; }
        public bool? IsSourceMainHero { get; set; }
        public bool? IsTargetMainHero { get; set; }
    }
}
