namespace StratzAPI.Models.MatchEvents
{
    public class BuildingEvent
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? match { get; set; }
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
