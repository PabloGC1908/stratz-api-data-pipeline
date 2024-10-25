namespace StratzAPI.Models
{
    public class Serie
    {
        public long Id { get; set; }
        public int? LeagueId { get; set; }
        public League? League { get; set; }
        public long? MatchId { get; set; }
        public Match? Match { get; set; }
        public string? Type { get; set; }
        public string? Phase { get; set; }
    }
}
