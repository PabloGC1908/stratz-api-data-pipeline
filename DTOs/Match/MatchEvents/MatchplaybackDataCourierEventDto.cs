namespace StratzAPI.DTOs.Match.MatchEvents
{
    public class MatchplaybackDataCourierEventDto
    {
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
