namespace StratzAPI.DTOs.Player
{
    public class ProSteamAccountDto
    {
        public string? Name { get; set; }
        public string? RealName { get; set; }
        public int TeamId { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsPro {  get; set; }
        public int? TotalEarnings { get; set; }
        public long? Birthday { get; set; }
        public string? Position { get; set; }
    }
}
