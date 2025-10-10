using StratzAPI.DTOs.Match.Events;

namespace StratzAPI.Models.Events
{
    public class InventoryEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? Item0 { get; set; }
        public int? Item0Charges { get; set; }
        public short? Item1 { get; set; }
        public int? Item1Charges { get; set; }
        public short? Item2 { get; set; }
        public int? Item2Charges { get; set; }
        public short? Item3 { get; set; }
        public int? Item3Charges { get; set; }
        public short? Item4 { get; set; }
        public int? Item4Charges { get; set; }
        public short? Item5 { get; set; }
        public int? Item5Charges { get; set; }
        public short? BackPack0 { get; set; }
        public int? BackPack0Charges { get; set; }
        public short? BackPack1 { get; set; }
        public int? BackPack1Charges { get; set; }
        public short? BackPack2 { get; set; }
        public int? BackPack2Charges { get; set; }
        public short? Teleport0 { get; set; }
        public int? Teleport0Charges { get; set; }
        public short? Neutral0 { get; set; }
        public int? Neutral0Charges { get; set; }
    }
}
