namespace StratzAPI.Models.MatchEvents
{
    public class RoshanEvent
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? match { get; set; }
        public int? Time { get; set; }
        public int? Hp { get; set; }
        public int? MaxHp { get; set; }
        public int? CreateTime { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? TotalDamageTaken { get; set; }
        public int? Item0 { get; set; }
        public int? Item1 { get; set; }
        public int? Item2 { get; set; }
        public int? Item3 { get; set; }
        public int? Item4 { get; set; }
        public int? Item5 { get; set; }
    }
}
