namespace StratzAPI.DTOs.Match
{
    public class MatchPickBansDto
    {
        public bool IsPick { get; set; }
        public int HeroId { get; set; }
        public int Order { get; set; }
        public bool IsRadiant { get; set; }
        public int PlayerIndex { get; set; }
        public bool? IsCaptain { get; set; }
    }
}
