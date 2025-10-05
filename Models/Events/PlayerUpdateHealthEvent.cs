namespace StratzAPI.Models.Events
{
    public class PlayerUpdateHealthEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public int? Hp { get; set; }
        public int? MaxHp { get; set; }
        public int? Mp { get; set; }
        public int? MaxMp { get; set; }
    }
}
