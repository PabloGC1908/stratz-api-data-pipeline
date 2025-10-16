namespace StratzAPI.Models.MatchEvents
{
    public class CourierEvent
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? match { get; set; }
        public int? OwnerHero { get; set; }
        public bool? IsRadiant { get; set; }
        public int? Time { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public int? Hp { get; set; }
        public bool? IsFlying { get; set; }
        public int? RespawnTime { get; set; }
        public bool? DidCastBoost { get; set; }
        public int? Item0Id { get; set; }
        public int? Item1Id { get; set; }
        public int? Item2Id { get; set; }
        public int? Item3Id { get; set; }
        public int? Item4Id { get; set; }
        public int? Item5Id { get; set; }
    }
}
