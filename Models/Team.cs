using System.ComponentModel.DataAnnotations;

namespace StratzAPI.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsPro { get; set; }
        public bool? IsLocked { get; set; }
        public string? CountryCode { get; set; }
        public string? Url { get; set; }
        public string? Logo { get; set; }
        public string? BaseLogo { get; set; }
        public string? BannerLogo { get; set; }
        public string? CountryName { get; set; }
    }
}
