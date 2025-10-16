namespace StratzAPI.DTOs.Match.Events
{
    public class CsEventDto
    {
        public int Time { get; set; }
        public short? Attacker { get; set; }
        public bool? IsFromIllusion { get; set; }
        public short? NpcId { get; set; }
        public short? ByAbility { get; set; }
        public short? ByItem { get; set; }
        public int? Gold { get; set; }
        public int? Xp { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public bool? IsCreep { get; set; }
        public bool? IsNeutral { get; set; }
        public bool? IsAncient { get; set; }
        public string? MapLocation { get; set; }
    }
}
