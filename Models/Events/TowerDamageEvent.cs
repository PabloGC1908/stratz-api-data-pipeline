namespace StratzAPI.Models.Events
{
    public class TowerDamageEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public short? NpcId { get; set; }
        public int? Damage { get; set; }
        public short? ByAbility { get; set; }
        public short? ByItem { get; set; }
        public short? FromNpc { get; set; }
    }
}
