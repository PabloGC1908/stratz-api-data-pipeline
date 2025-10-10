using StratzAPI.DTOs.Match.Events;

namespace StratzAPI.Models.Events
{
    public class SpiritBearInventoryEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? Item0 { get; set; }
        public short? Item1 { get; set; }
        public short? Item2 { get; set; }
        public short? Item3 { get; set; }
        public short? Item4 { get; set; }
        public short? Item5 { get; set; }
        public short? BackPack0 { get; set; }
        public short? BackPack1 { get; set; }
        public short? BackPack2 { get; set; }
        public short? Teleport0 { get; set; }
        public short? Neutral0 { get; set; }
    }
}
