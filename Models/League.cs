namespace StratzAPI.Models
{
    public class League
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Banner { get; set; }
        public decimal? BasePrizePool { get; set; }
        public DateTime? StopSalesTime { get; set; }
        public string? Tier { get; set; }
        public string? Region { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? FreeToSpectate { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? TournamentUrl { get; set; }
        public DateTime? LastMatchDate { get; set; }
        public bool? HasLiveMatches { get; set; }
        public decimal PrizePool { get; set; }
        public string? ImageUri { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Country { get; set; }
        public string? Venue { get; set; }
    }
}
