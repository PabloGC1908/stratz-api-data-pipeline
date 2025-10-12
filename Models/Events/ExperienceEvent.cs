namespace StratzAPI.Models.Events
{
    public class ExperienceEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? Amount { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
    }
}
