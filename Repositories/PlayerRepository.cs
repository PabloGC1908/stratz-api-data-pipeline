using StratzAPI.Data;
using StratzAPI.DTOs.Player;
using StratzAPI.Models;
using StratzAPI.Services;
using System.Numerics;

namespace StratzAPI.Repositories
{
    public class PlayerRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlayerRepository> _logger;
        private readonly GraphQLService _graphQLService;

        public PlayerRepository(AppDbContext context, ILogger<PlayerRepository> logger, GraphQLService graphQLService)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
        }

        public async Task<Player> GetOrFetchPlayerAsync(long steamAccountId)
        {
            _logger.LogInformation("Verificando si el player con id {steamAccountId} se encuentra en la base de datos", steamAccountId);
            Player? player = await _context.Player.FindAsync(steamAccountId);

            if (player == null)
            {
                _logger.LogInformation("El jugador no se encuentra en la base de datos, haciendo una peticion a la API");
                return await FetchAndSavePlayer(steamAccountId);
            }
            else
            {
                _logger.LogInformation("El jugador se encuentra en la base de datos");
                return player;
            }
        }

        public async Task<Player> FetchAndSavePlayer(long steamAccountId)
        {

            const string query = @"
            query($steamAccountId: Long!) {
                player(steamAccountId: $steamAccountId) {
                    steamAccountId
                    steamAccount {
                        countryCode
                        proSteamAccount {
                            name
                            realName
                            isLocked
                            isPro
                            totalEarnings
                            birthday
                            position
                        }
                    }
                }
            }";

            _logger.LogInformation("Extrayendo data del jugador con id: {steamAccountId}", steamAccountId);
            var playerData = await _graphQLService.SendGraphQLQueryAsync<PlayerResponseType>(query, 
                            new { steamAccountId }) ?? throw new Exception("No se extrajo la data del jugador correctamente");
            _logger.LogInformation("Data del jugador con id {steamAccountId} extraida correctamente", steamAccountId);

            Player player = Map(playerData.Player);

            return player;
        }


        public Player Map(PlayerDto playerDto)
        {
            _logger.LogInformation("Iniciando mapeo de datos del jugador");

            try
            {
                Player player = new()
                {
                    Id = playerDto.SteamAccountId,
                    Name = playerDto?.SteamAccount?.ProSteamAccount?.Name ?? null,
                    RealName = playerDto?.SteamAccount?.ProSteamAccount?.RealName ?? null,
                    IsLocked = playerDto?.SteamAccount?.ProSteamAccount?.IsLocked ?? null,
                    IsPro = playerDto?.SteamAccount?.ProSteamAccount?.IsPro ?? null,
                    TotalEarnings = playerDto?.SteamAccount?.ProSteamAccount?.TotalEarnings ?? null,
                    Birthday = Utils.ConvertUnixToDateTime(playerDto?.SteamAccount?.ProSteamAccount?.Birthday),
                    Position = playerDto?.SteamAccount?.ProSteamAccount?.Position ?? null,
                    CountryCode = playerDto?.SteamAccount?.CountryCode ?? null,
                };

                _logger.LogInformation("Mapeo exitoso para el jugador: {player.Id}, {player.Name}", player.Id, player.Name);

                return player;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el mapeo del jugador con id: {playerDto.SteamAccountId}", playerDto.SteamAccountId);
                throw;
            }
        }
    }
}
