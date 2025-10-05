namespace StratzAPI.Models.Events
{
    public class PlayerUpdateLevelEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? Level { get; set; }
    }
}
