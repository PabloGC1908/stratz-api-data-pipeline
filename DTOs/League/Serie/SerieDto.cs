namespace StratzAPI.DTOs.League.Serie
{
    public class SerieDto
    {
        public long Id { get; set; }
        public ICollection<MatchIdDto>? Matches { get; set; }
        public string? Type { get; set; }
    }
}
