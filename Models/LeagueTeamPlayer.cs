namespace StratzAPI.Models
{
    public class LeagueTeamPlayer
    {
        public long Id { get; set; }
        public int LeagueId { get; set; }
        public required League League { get; set; }
        public int TeamId { get; set; }
        public required Team Team { get; set; }
        public long PlayerId { get; set; }
        public required Player Player { get; set; }
    }
}
