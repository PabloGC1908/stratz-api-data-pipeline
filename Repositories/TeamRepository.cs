using StratzAPI.Data;
using StratzAPI.DTOs.Team;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories
{
    public class TeamRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MatchRepository> _logger;
        private readonly GraphQLService _graphQLService;

        public TeamRepository(AppDbContext context, ILogger<MatchRepository> logger, GraphQLService graphQLService)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
        }

        public async Task GetTeamData(int teamId)
        {
            const string query = @"
            query($teamId: Int!) {
                team(teamId: $teamId) {
                    id
                    name
                    tag
                    dateCreated
                    isPro
                    isLocked
                    countryCode
                    url
                    logo
                    baseLogo
                    bannerLogo
                    countryName
                }
            }";

            _logger.LogInformation("Extrayendo data del equipo con id {teamId} de la API", teamId);
            var teamData = await _graphQLService.SendGraphQLQueryAsync<TeamResponseType>(query, new { teamId });

            if (teamData == null)
            {
                _logger.LogError("No se extrajo la data correctamente");
                return;
            }

            Team team = Map(teamData.Team);
            await AddTeamAsync(team);
        }

        public Team Map(TeamDto teamDto)
        {
            return new Team
            {
                Id = teamDto.Id,
                Name = teamDto.Name,
                Tag = teamDto.Tag,
                DateCreated = Utils.ConvertUnixToDateTime(teamDto.DateCreated),
                IsPro = teamDto.IsPro,
                IsLocked = teamDto.IsLocked,
                CountryCode = teamDto.CountryCode,
                Url = teamDto.Url,
                Logo = teamDto.Logo,
                BaseLogo = teamDto.BaseLogo,
                BannerLogo = teamDto.BannerLogo,
                CountryName = teamDto.CountryName
            };
        }

        public async Task AddTeamAsync(Team team)
        {
            _context.Team.Add(team);
            await _context.SaveChangesAsync();
        }

        public bool GetTeam(long teamId) 
        {
            return _context.Team.Any(x => x.Id == teamId);
        }
    }
}
