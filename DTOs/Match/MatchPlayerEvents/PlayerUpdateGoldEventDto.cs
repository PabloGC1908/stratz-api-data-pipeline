namespace StratzAPI.DTOs.Match.Events
{
    public class PlayerUpdateGoldEventDto
    {
        public int Time { get; set; }
        public int? Gold { get; set; }
        public int? UnreliableGold { get; set; }
        public int? Networth { get; set; }
        public int? NetworthDifference { get; set; }
    }
}
