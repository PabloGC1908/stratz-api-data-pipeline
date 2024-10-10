namespace StratzAPI.Models
{
    public class MatchPlayer
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match Match { get; set; }
        public long SteamAccountId { get; set; }
        public Player Player { get; set; }
        public bool IsRadiant { get; set; }
        public byte HeroId { get; set; }
        public byte Kills { get; set; }
        public byte Deaths { get; set; }
        public byte Assists { get; set; }
        public short NumLastHits { get; set; }
        public short NumDenies { get; set; }
        public short GoldPerMinute { get; set; }
        public int Networth { get; set; }
        public short ExperiencePerMinute { get; set; }
        public short Level { get; set; }
        public int Gold { get; set; }
        public int GoldSpent { get; set; }
        public int HeroDamage { get; set; }
        public int TowerDamage { get; set; }
        public int HeroHealing { get; set; }
        public string Lane { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string RoleBasic { get; set; }
        public string Award { get; set; }

        public MatchPlayerItems MatchPlayerItems { get; set; }
    }
}
