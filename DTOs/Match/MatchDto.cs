namespace StratzAPI.DTOs.Match
{
    public class MatchDto
    {
        public long Id { get; set; }
        public bool DidRadiantWin { get; set; }
        public int DurationSeconds { get; set; }
        public long StartDateTime { get; set; }
        public long EndDateTime { get; set; }
        public int? FirstBloodTime { get; set; }
        public int? LeagueId { get; set; }
        public int? RadiantTeamId { get; set; }
        public int? DireTeamId { get; set; }
        public int? GameVersionId { get; set; }
        public ICollection<decimal?>? WinRates { get; set; }
        public ICollection<decimal?>? PredictedWinRates { get; set; }
        public ICollection<int?>? RadiantKills { get; set; }
        public ICollection<int?>? DireKills { get; set; }
        public ICollection<int?>? RadiantNetworthLeads { get; set; }
        public ICollection<int?>? RadiantExperienceLeads { get; set; }
        public ICollection<MatchPickBansDto>? PickBans { get; set; }
        public ICollection<MatchPlayerDto>? Players { get; set; }
    }
}
