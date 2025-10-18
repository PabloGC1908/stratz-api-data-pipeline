namespace StratzAPI.DTOs.Match
{
    public class MatchUpdateDto
    {
        public long Id { get; set; }
        public MatchPlaybackDataDto? PlaybackData { get; set; }
    }
}
