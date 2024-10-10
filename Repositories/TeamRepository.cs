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
        private readonly PlayerRepository _playerRepository;

        public TeamRepository(AppDbContext context, ILogger<MatchRepository> logger,
                                GraphQLService graphQLService, PlayerRepository playerRepository)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
            _playerRepository = playerRepository;
        }

        public async Task CreateAndSaveTeamPlayersQuery(int teamId, League league)
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
                    members {
                        steamAccountId
                    }
                }
            }";

            _logger.LogInformation("Extrayendo data del equipo con id {teamId} de la API", teamId);
            var teamData = await _graphQLService.SendGraphQLQueryAsync<TeamResponseType>(query, new { teamId });

            if (teamData == null)
            {
                _logger.LogError("No se extrajo la data correctamente");
                return;
            }

            _logger.LogInformation("Data del equipo con id {teamId} extraida correctamente", teamId);

            try
            {
                Team team = Map(teamData.Team);
                await AddTeamAsync(team);

                var savedTeam = _context.Team.Find(team.Id);
                if (savedTeam == null)
                {
                    _logger.LogError("El equipo con id {team.Id} no fue guardado correctamente.", team.Id);
                    return;
                }

                _logger.LogInformation("El equipo con id {teamId} fue guardado correctamente", teamId);

                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    foreach (var playerId in teamData.Team.Members)
                    {
                        Player player = await _playerRepository.FetchAndSavePlayer(playerId.SteamAccountId);
                        Team? playerTeam = await _context.Team.FindAsync(player.TeamId);

                        if (playerTeam == null)
                        {
                            _logger.LogInformation("El jugador se encuentra en otro equipo, probablemente por que es standim," +
                                                        " añadiendo su equipo a la base de datos");
                            await CreateAndSaveTeamQuery((int)player.TeamId);
                        }
                        
                        await _playerRepository.AddPlayerAsync(player);
                        await AddLeagueTeamPlayer(league, team, player);
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Error al guardar los jugadores: {ex.Message}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al guardar el equipo o los jugadores: {ex.Message}", ex.Message);
            }
        }

        public async Task CreateAndSaveTeamQuery(int teamId)
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

            _logger.LogInformation("Data del equipo con id {teamId} extraida correctamente", teamId);

            try
            {
                Team team = Map(teamData.Team);
                await AddTeamAsync(team);

                var savedTeam = _context.Team.Find(team.Id);
                if (savedTeam == null)
                {
                    _logger.LogError("El equipo con id {team.Id} no fue guardado correctamente.", team.Id);
                    return;
                }

                _logger.LogInformation("El equipo con id {teamId} fue guardado correctamente", teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el mapeo de TeamDto con ID: {Id}", teamData.Team.Id);
                throw;
            }
        }


        public Team Map(TeamDto teamDto)
        {
            _logger.LogInformation("Iniciando mapea de datos del equipo");
            try
            {
                var team = new Team
                {
                    Id = teamDto.Id,
                    Name = teamDto.Name,
                    Tag = teamDto.Tag,
                    DateCreated = Utils.ConvertUnixToDateTime(teamDto.DateCreated),
                    IsPro = teamDto.IsPro ?? false,
                    IsLocked = teamDto.IsLocked ?? false,
                    CountryCode = teamDto.CountryCode,
                    Url = teamDto.Url,
                    Logo = teamDto.Logo,
                    BaseLogo = teamDto.BaseLogo,
                    BannerLogo = teamDto.BannerLogo,
                    CountryName = teamDto.CountryName
                };

                _logger.LogInformation("Mapeo exitoso para Team con ID: {Id}, Nombre: {Name}", team.Id, team.Name);

                return team;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el mapeo de TeamDto con ID: {Id}", teamDto.Id);
                throw;
            }
        }

        public async Task AddTeamAsync(Team team)
        {
            _logger.LogInformation("Ingresando al equipo {team.Id} a la base de datos", team.Id);

            await _context.Team.AddAsync(team);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Se añadio el equipo a la base de datos");
        }

        public async Task AddLeagueTeamPlayer(League league, Team team, Player player)
        {
            _logger.LogInformation("Añadiendo la data extraida a la tabla LeagueTeamPlayer");
            var leagueTeamPlayer = new LeagueTeamPlayer
            {
                LeagueId = league.Id,
                League = league,
                TeamId = team.Id,
                Team = team,
                PlayerId = player.Id,
                Player = player
            };

            await _context.LeagueTeamPlayer.AddAsync(leagueTeamPlayer);

            _logger.LogInformation("Se añadio la data a la base de datos correctamente");
        }
    }
}
