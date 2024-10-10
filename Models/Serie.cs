namespace StratzAPI.Models
{
    public class Serie
    {
        public long Id { get; set; }
        public int LeagueId { get; set; }
        public required League League { get; set; }
    }
}
