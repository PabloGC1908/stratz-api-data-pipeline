using StratzAPI.Data;
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

        public async Task ProcessPlaybackMatchPlayerData(ICollection<DTOs.Match.PlaybackDataDto> playbackData)
        {

        }
    }
}
