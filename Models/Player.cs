using System.ComponentModel.DataAnnotations;

namespace StratzAPI.Models
{
    public class Player
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? RealName { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsPro { get; set; }
        public int? TotalEarnings { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Position { get; set; }
        public string? CountryCode { get; set; }
    }
}
