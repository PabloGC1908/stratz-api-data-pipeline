namespace StratzAPI.Models.Events
{
    public class PlayerUpdateBattleEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? DamageMinMax { get; set; }
        public int? DamageBonus { get; set; }
        public int? HpRegen { get; set; }
        public int? MpRegen { get; set; }
    }
}
