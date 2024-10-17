using Microsoft.EntityFrameworkCore;
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

        public async Task SaveTeamInLeague(int teamId, int leagueId)
        {
            League? league = _context.League.Find(leagueId);
            Team? team = _context.Team.Find(teamId);

            if(league == null)
            {
                return;
            }

            if (team == null)
            {
                return;
            }

            await FetchAndSavePlayers(teamId, league, team);
        }

        public async Task GetOrFetchTeam(int teamId, League league)
        {
            _logger.LogInformation("Viendo si el equipo {teamId} se encuentra en la base de datos", teamId);
            Team? team = await _context.Team.FindAsync(teamId);

            if (team != null)
            {
                _logger.LogInformation("El equipo esta en la base de datos, actualizando jugadores");
                await FetchAndSavePlayers(teamId, league, team);
            }
            else
            {
                _logger.LogInformation("El equipo no se esta en la base de datos, haciendo peticion a la API");
                await CreateAndSaveTeamPlayersQuery(teamId, league);
            }
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
                _logger.LogError("Error al guardar el equipo o los jugadores: {ex.Message}", ex.Message);
            }
        }

        public async Task FetchTeamQuery(int teamId)
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

        public async Task FetchAndSavePlayers(int teamId, League league, Team team)
        {
            const string query = @"
            query($teamId: Int!) {
                team(teamId: $teamId) {
                    members {
                        steamAccountId
                    }
                }
            }";

            _logger.LogInformation("Extrayendo data del equipo con id {teamId} de la API", teamId);
            var teamData = await _graphQLService.SendGraphQLQueryAsync<TeamMembersResponseType>(query, new { teamId });

            if(teamData == null)
            {
                throw new Exception("No se extrajo la data correctamente");
            }

            await SavePlayers(teamData, league, team);
        }

        public async Task SavePlayers(TeamMembersResponseType teamMembers, League league, Team team)
        {
            foreach (var playerId in teamMembers.Team.Members)
            {
                try
                {
                    var existingPlayer = await _context.Player.FirstOrDefaultAsync(p => p.Id == playerId.SteamAccountId);
                    Player player;

                    if (existingPlayer != null)
                    {
                        player = existingPlayer;
                        _logger.LogInformation("El jugador con SteamAccountId {playerId.SteamAccountId} ya existe en la base de datos.", playerId.SteamAccountId);
                    }
                    else
                    {
                        player = await _playerRepository.FetchAndSavePlayer(playerId.SteamAccountId);

                        if (player == null)
                        {
                            _logger.LogWarning("No se pudo obtener información del jugador con SteamAccountId {playerId.SteamAccountId}. Saltando...", playerId.SteamAccountId);
                            continue;
                        }

                        var playerTeam = await _context.Team.FindAsync(player.TeamId);

                        if (playerTeam == null)
                        {
                            _logger.LogInformation("No se encontro el equipo en la base de datos, añadiendo el equipo");
                            await FetchTeamQuery(player.TeamId);
                        }
                    }

                    var existingLeagueTeamPlayer = await _context.LeagueTeamPlayer
                        .FirstOrDefaultAsync(ltp => ltp.LeagueId == league.Id && ltp.TeamId == team.Id && ltp.PlayerId == player.Id);

                    if (existingLeagueTeamPlayer != null)
                    {
                        _logger.LogInformation("El jugador con id {player.Id} ya está registrado en el equipo {team.Id} para la liga {league.Id}.", player.Id, team.Id, league.Id);
                        continue;
                    }

                    await AddLeagueTeamPlayer(league, team, player);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error al procesar el jugador con SteamAccountId {playerId.SteamAccountId}: {ex.Message}", playerId.SteamAccountId, ex.Message);
                }
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
