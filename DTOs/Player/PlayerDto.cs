namespace StratzAPI.DTOs.Player
{
    public class PlayerDto
    {
        public long SteamAccountId { get; set; }
        public SteamAccountDto? SteamAccount { get; set; }
    }
}
