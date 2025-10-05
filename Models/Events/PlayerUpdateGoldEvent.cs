namespace StratzAPI.Models.Events
{
    public class PlayerUpdateGoldEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? Gold { get; set; }
        public int? UnreliableGold { get; set; }
        public int? Networth { get; set; }
        public int? NetworthDifference { get; set; }
    }
}
