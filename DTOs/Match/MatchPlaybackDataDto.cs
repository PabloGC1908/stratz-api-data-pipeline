using StratzAPI.DTOs.Match.Events;
using StratzAPI.DTOs.Match.MatchEvents;

namespace StratzAPI.DTOs.Match
{
    public class MatchPlaybackDataDto
    {
        public List<CourierEventDto> CourierEvents { get; set; }
        public List<MatchRuneEventDto> RuneEvents { get; set; }
        public List<WardEventDto> WardEvents { get; set; }
        public List<BuildingEventDto> BuildingEvents { get; set; }
        public List<TowerDeathEventDto> TowerDeathEvents { get; set; }
        public List<RoshanEventDto> RoshanEvents { get; set; }
    }
}
