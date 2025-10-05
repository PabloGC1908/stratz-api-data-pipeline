using StratzAPI.DTOs.Match.Events;

namespace StratzAPI.Models.Events
{
    public class InventoryEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public ItemSlotDto? Item0 { get; set; }
        public ItemSlotDto? Item1 { get; set; }
        public ItemSlotDto? Item2 { get; set; }
        public ItemSlotDto? Item3 { get; set; }
        public ItemSlotDto? Item4 { get; set; }
        public ItemSlotDto? Item5 { get; set; }
        public ItemSlotDto? BackPack0 { get; set; }
        public ItemSlotDto? BackPack1 { get; set; }
        public ItemSlotDto? BackPack2 { get; set; }
        public ItemSlotDto? Teleport0 { get; set; }
        public ItemSlotDto? Neutral0 { get; set; }
    }
}
