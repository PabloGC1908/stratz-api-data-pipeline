using StratzAPI.Data;
using StratzAPI.DTOs;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories
{
    public class MatchRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MatchRepository> _logger;
        private readonly GraphQLService _graphQLService;

        public MatchRepository(AppDbContext context, ILogger<MatchRepository> logger, GraphQLService graphQLService)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
        }

        public void GetMatchData(long matchId)
        {

        }

        public void AddMatch(Match match)
        {
            
        }

        public void FindMatch(long  id)
        {

        }
    }
}
