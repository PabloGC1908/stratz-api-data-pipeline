using StratzAPI.Data;
using StratzAPI.DTOs.League;
using StratzAPI.DTOs.Team;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories
{
    public class LeagueRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LeagueRepository> _logger;
        private readonly GraphQLService _graphQLService;

        public LeagueRepository(AppDbContext context, ILogger<LeagueRepository> logger, GraphQLService graphQLService)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
        }

        public async Task GetLeagueData(int leagueId)
        {
            const string query = @"
            query($id: Int!) {
                league(id: $teamId) {
                    id
                    name
                    banner
                    basePrizePool
                    stopSalesTime
                    tier
                    region
                    private
                    freeToSpectate
                    startDateTime
                    endDateTime
                    tournamentUrl
                    lastMatchDate
                    hasLiveMatches
                    prizePool
                    imageUri
                    displayName
                    description
                    country
                    venue
                }
            }";

            _logger.LogInformation("Extrayendo data del equipo con id {leagueId} de la API", leagueId);
            var leagueData = await _graphQLService.SendGraphQLQueryAsync<LeagueResponseType>(query, new { leagueId });

            if (leagueData == null)
            {
                _logger.LogError("No se extrajo la data correctamente");
                return;
            }

            League league = Map(leagueData.League);
            await AddLeagueAsync(league);
        }

        public League Map(LeagueDto leagueDto)
        {
            return new League
            {

            };
        }

        public async Task AddLeagueAsync(League league)
        {
            _context.Add(league);
            await _context.SaveChangesAsync();
        }

        public bool GetTeam(int id)
        {
            return _context.League.Any(x => x.Id == id);
        }
    }
}
