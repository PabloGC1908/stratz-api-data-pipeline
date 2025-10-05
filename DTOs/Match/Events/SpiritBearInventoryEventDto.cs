namespace StratzAPI.DTOs.Match.Events
{
    public class SpiritBearInventoryEventDto
    {
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
