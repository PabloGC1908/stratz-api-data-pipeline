namespace StratzAPI.Models.Events
{
    public class AbilityLearnEvent
    {
        public long Id { get; set; }
        public long MatchPlayerId { get; set; }
        public MatchPlayer? MatchPlayer { get; set; }
        public int Time { get; set; }
        public short? AbilityId { get; set; }
        public int? LevelObtained { get; set; }
        public int? Level { get; set; }
        public bool? IsUltimate { get; set; }
        public bool? IsTalent { get; set; }
        public bool? IsMaxLevel { get; set; }
    }
}
