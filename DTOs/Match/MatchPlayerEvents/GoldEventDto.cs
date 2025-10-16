namespace StratzAPI.DTOs.Match.Events
{
    public class GoldEventDto
    {
        public int Time { get; set; }
        public int? Amount { get; set; }
        public string? Reason { get; set; }
        public int? NpcId { get; set; }
        public bool? IsValidForStats { get; set; }
    }
}
