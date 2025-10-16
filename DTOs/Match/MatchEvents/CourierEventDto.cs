namespace StratzAPI.DTOs.Match.MatchEvents
{
    public class CourierEventDto
    {
        public int Id { get; set; }
        public int? OwnerHero { get; set; }
        public bool? IsRadiant { get; set; }
        public List<MatchplaybackDataCourierEventDto>? Events { get; set; }
    }
}
