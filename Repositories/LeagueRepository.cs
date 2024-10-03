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
                Id = leagueDto.Id,
                Name = leagueDto.Name,
                Banner = leagueDto.Banner,
                BasePrizePool = leagueDto.BasePrizePool,
                StopSalesTime = Utils.ConvertUnixToDateTime(leagueDto.StopSalesTime),
                Tier = leagueDto.Tier,
                Region = leagueDto.Region,
                IsPrivate = leagueDto.IsPrivate,
                FreeToSpectate = leagueDto.FreeToSpectate,
                StartDateTime = Utils.ConvertUnixToDateTime(leagueDto.StartDateTime),
                EndDateTime = Utils.ConvertUnixToDateTime(leagueDto.EndDateTime),
                TournamentUrl = leagueDto.TournamentUrl,
                PrizePool = leagueDto.PrizePool,
                ImageUri = leagueDto.ImageUri,
                DisplayName = leagueDto.DisplayName,
                Description = leagueDto.Description,
                Country = leagueDto.Country,
                Venue = leagueDto.Venue
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
