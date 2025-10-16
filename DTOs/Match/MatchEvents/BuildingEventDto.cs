namespace StratzAPI.DTOs.Match.MatchEvents
{
    public class BuildingEventDto
    {
        public int? Time { get; set; }
        public int? IndexId { get; set; }
        public string? Type { get; set; }
        public int? Hp { get; set; }
        public int? MaxHp { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public bool? IsRadiant { get; set; }
        public int? NpcId { get; set; }
        public bool? DidShrineActivate { get; set; }
    }
}
