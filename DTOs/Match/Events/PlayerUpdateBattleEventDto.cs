namespace StratzAPI.DTOs.Match.Events
{
    public class PlayerUpdateBattleEventDto
    {
        public int Time { get; set; }
        public int? DamageMinMax { get; set; }
        public int? DamageBonus { get; set; }
        public int? HpRegen { get; set; }
        public int? MpRegen { get; set; }
    }
}
