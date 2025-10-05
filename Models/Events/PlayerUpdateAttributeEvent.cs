namespace StratzAPI.Models.Events
{
    public class PlayerUpdateAttributeEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? Agi { get; set; }
        public int? Int { get; set; }
        public int? Str { get; set; }
    }
}
