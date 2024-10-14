using StratzAPI.Data;
using StratzAPI.DTOs.League;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories
{
    public class LeagueRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LeagueRepository> _logger;
        private readonly GraphQLService _graphQLService;
        private readonly TeamRepository _teamRepository;

        public LeagueRepository(AppDbContext context, ILogger<LeagueRepository> logger,
                                GraphQLService graphQLService, TeamRepository teamRepository)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
            _teamRepository = teamRepository;
        }

        public async Task GetOrFetchLeagueAsync(int leagueId)
        {
            _logger.LogInformation("Viendo si el torneo {leagueId} se encuentra en la base de datos", leagueId);
            League? league = await _context.League.FindAsync(leagueId);

            if (league != null)
            {
                _logger.LogInformation("La liga si esta presente en la base de datos, actualizando datos de los equipos");
                await ExtractTeamsByLeagueId(leagueId, league);
            } 
            else
            {
                _logger.LogInformation("La liga con id {leagueId} no se encuentra en la base de datos, extrayendo datos", leagueId);
                await GetAndSaveLeagueData(leagueId);
            }
        }

        public async Task GetAndSaveLeagueData(int leagueId)
        {
            const string query = @"
            query($leagueId: Int!) {
                league(id: $leagueId) {
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

            _logger.LogInformation("Extrayendo data de la liga con id {leagueId} de la API", leagueId);
            var leagueData = await _graphQLService.SendGraphQLQueryAsync<LeagueResponseType>(query, new { leagueId });

            if (leagueData == null)
            {
                _logger.LogError("No se extrajo la data correctamente");
                return;
            }

            _logger.LogInformation("Se extrajo la data del torneo con id: {leagueId} de nombre: {leagueData.League.DisplayName}",
                                                leagueId, leagueData.League.DisplayName);

            League league = Map(leagueData.League);
            await AddLeagueAsync(league);
            await ExtractTeamsByLeagueId(leagueId, league);
        }

        public async Task ExtractTeamsByLeagueId(int leagueId, League league)
        {
            const string query = @"
            query($leagueId: Int!) {
                league(id: $leagueId) {
                    tables {
                        tableTeams {
                            teamId
                        }
                    }
                }
            }";

            _logger.LogInformation("Extrayendo data de la liga con id {leagueId} de la API", leagueId);
            var leagueData = await _graphQLService.SendGraphQLQueryAsync<LeagueTeamReponseType>(query, new { leagueId });

            if (leagueData == null)
            {
                throw new Exception("Error en la extraccion de la data");
            }

            await HandleLeagueTeamsData(leagueData, league);
        }

        public async Task HandleLeagueTeamsData(LeagueTeamReponseType leagueReponse, League league)
        {
            foreach (var team in leagueReponse.League.Tables.TableTeams)
            {
                _logger.LogInformation("Ingresando data del equipo con id {team.teamId} que participa" +
                                                " en la liga con id {league.Id}", team.TeamId, league.Id);
                await _teamRepository.GetOrFetchTeam(team.TeamId, league);
            }
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
            _context.League.Add(league);
            await _context.SaveChangesAsync();
        }
    }
}
