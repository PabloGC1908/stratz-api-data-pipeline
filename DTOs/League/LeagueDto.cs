namespace StratzAPI.DTOs.League
{
    public class LeagueDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Banner { get; set; }
        public decimal? BasePrizePool { get; set; }
        public long? StopSalesTime { get; set; }
        public string? Tier { get; set; }
        public string? Region { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? FreeToSpectate { get; set; }
        public long StartDateTime { get; set; }
        public long EndDateTime { get; set; }
        public string? TournamentUrl { get; set; }
        public decimal PrizePool { get; set; }
        public string? ImageUri { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Country { get; set; }
        public string? Venue { get; set; }
    }
}
