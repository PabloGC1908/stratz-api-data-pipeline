namespace StratzAPI.DTOs.Match
{
    public class MatchPlayerDto
    {
        public long SteamAccountId { get; set; }
        public bool IsRadiant { get; set; }
        public bool IsVictory { get; set; }
        public short HeroId { get; set; }
        public byte Kills { get; set; }
        public byte Deaths { get; set; }
        public byte Assists { get; set; }
        public short NumLastHits { get; set; }
        public short NumDenies { get; set; }
        public short GoldPerMinute { get; set; }
        public int Networth { get; set; }
        public short ExperiencePerMinute { get; set; }
        public byte Level { get; set; }
        public int Gold { get; set; }
        public int GoldSpent { get; set; }
        public int HeroDamage { get; set; }
        public int TowerDamage { get; set; }
        public int HeroHealing { get; set; }
        public string? Lane { get; set; }
        public string? Position { get; set; }
        public string? Rol { get; set; }
        public string? RolBasic { get; set; }
        public string? Award { get; set; }
        public short? Item0Id { get; set; }
        public short? Item1Id { get; set; }
        public short? Item2Id { get; set; }
        public short? Item3Id { get; set; }
        public short? Item4Id { get; set; }
        public short? Item5Id { get; set; }
        public short? Item6Id { get; set; }
        public short? Backpack0Id { get; set; }
        public short? Backpack1Id { get; set; }
        public short? Backpack2Id { get; set; }
        public short? Neutral0Id { get; set; }
        public PlaybackDataDto PlayBackData { get; internal set; }
    }
}
