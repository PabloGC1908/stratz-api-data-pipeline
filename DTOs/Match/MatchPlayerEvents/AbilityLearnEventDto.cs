namespace StratzAPI.DTOs.Match.Events
{
    public class AbilityLearnEventDto
    {
        public int Time { get; set; }
        public short? AbilityId { get; set; }
        public int? LevelObtained { get; set; }
        public int? Level { get; set; }
        public bool? IsUltimate { get; set; }
        public bool? IsTalent { get; set; }
        public bool? IsMaxLevel { get; set; }
    }
}
