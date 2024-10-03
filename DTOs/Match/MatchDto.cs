namespace StratzAPI.DTOs.Match
{
    public class MatchDto
    {
        public long Id { get; set; }
        public bool DidRadiantWin { get; set; }
        public int DurationSeconds { get; set; }
        public long StartDateTime { get; set; }
        public long EndDateTime { get; set; }
        public int FirstBloodTime { get; set; }
        public int LeagueId { get; set; }
        public long RadiantTeamId { get; set; }
        public long DireTeamId { get; set; }
        public int GameVersionId { get; set; }
        public List<double>? WinRates { get; set; }
        public List<double>? PredictedWinRates { get; set; }
        public List<int>? RadiantKills { get; set; }
        public List<int>? DireKills { get; set; }
        public List<int>? RadiantNetworthLeads { get; set; }
        public List<int>? RadiantExperienceLeads { get; set; }
        public List<MatchPickBansDto>? PickBans { get; set; }
        public List<MatchPlayerDto>? Players { get; set; }
    }
}
