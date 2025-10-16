using StratzAPI.DTOs.Match.Events;

namespace StratzAPI.DTOs.Match
{
    public class MatchPlaybackDataDto
    {
        public List<CourierEventDto> CourierEvents { get; set; }
        public List<RuneEventDto> RuneEvents { get; set; }
        public List<WardEventDto> WardEvents { get; set; }
        public List<BuildingEventDto> BuildingEvents { get; set; }
        public List<TowerDeathEventDto> TowerDeathEvents { get; set; }
        public List<RoshanEventDto> RoshanEvents { get; set; }
    }
}
