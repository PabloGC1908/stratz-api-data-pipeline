using StratzAPI.DTOs.Match.Events;

namespace StratzAPI.Models.Events
{
    public class SpiritBearInventoryEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public ItemIdDto? Item0 { get; set; }
        public ItemIdDto? Item1 { get; set; }
        public ItemIdDto? Item2 { get; set; }
        public ItemIdDto? Item3 { get; set; }
        public ItemIdDto? Item4 { get; set; }
        public ItemIdDto? Item5 { get; set; }
        public ItemIdDto? BackPack0 { get; set; }
        public ItemIdDto? BackPack1 { get; set; }
        public ItemIdDto? BackPack2 { get; set; }
        public ItemIdDto? Teleport0 { get; set; }
        public ItemIdDto? Neutral0 { get; set; }
    }
}
