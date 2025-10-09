using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.DTOs.Match.Events;
using StratzAPI.Models.Events;
using StratzAPI.Services;

namespace StratzAPI.Repositories
{
    public class PlaybackDataRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlaybackDataRepository> _logger;
        private readonly GraphQLService _graphQLService;

        public PlaybackDataRepository(AppDbContext context, ILogger<PlaybackDataRepository> logger,
            GraphQLService graphQLService)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
        }

        public async Task ProcessPlaybackMatchPlayerData(ICollection<PlaybackDataDto> playbackData, long matchPlayerId)
        {
            foreach (var abilityLearnEventsDto in playbackData)
            {

            }
        }

        public AbilityLearnEvent mapAbilityLearnEvent(AbilityLearnEventDto abilityLearnEventDto, long matchPlayerId)
        {
            return new AbilityLearnEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = abilityLearnEventDto.Time,
                AbilityId = abilityLearnEventDto.AbilityId,
                LevelObtained = abilityLearnEventDto.LevelObtained,
                Level = abilityLearnEventDto.Level,
                IsUltimate = abilityLearnEventDto.IsUltimate,
                IsTalent = abilityLearnEventDto.IsTalent,
                IsMaxLevel = abilityLearnEventDto.IsMaxLevel
            };
        }

        public AbilityUsedEvent mapAbilityUsedEvent(AbilityUsedEventDto abilityUsedEventDto, long matchPlayerId)
        {
            return new AbilityUsedEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = abilityUsedEventDto.Time,
                AbilityId = abilityUsedEventDto.AbilityId,
                Attacker = abilityUsedEventDto.Attacker,
                Target = abilityUsedEventDto.Target
            };
        }

        public AssistEvent mapAssistEvent(AssistEventDto assistEventDto)
        {

        }

    }
}
