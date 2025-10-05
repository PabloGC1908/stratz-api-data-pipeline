namespace StratzAPI.DTOs.Match.Events
{
    public class KillEventDto
    {
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public bool? IsFromIllusion { get; set; }
        public short? Target { get; set; }
        public short? ByAbility { get; set; }
        public short? ByItem { get; set; }
        public int? Gold { get; set; }
        public int? Xp { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public List<int>? Assist { get; set; }
        public bool? IsSolo { get; set; }
        public bool? IsGank { get; set; }
        public bool? IsInvisible { get; set; }
        public bool? IsSmoke { get; set; }
        public bool? IsTpRecently { get; set; }
        public bool? IsRuneEffected { get; set; }
    }
}
