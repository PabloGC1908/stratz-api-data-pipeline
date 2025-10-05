namespace StratzAPI.Models.Events
{
    public class PlayerUpdatePositionEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
    }
}
