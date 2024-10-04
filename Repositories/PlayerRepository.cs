using StratzAPI.Data;
using StratzAPI.DTOs.Player;
using StratzAPI.Models;
using StratzAPI.Services;

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

        public async Task GetPlayerData(long steamAccountId)
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
                            teamId
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
            var playerData = await _graphQLService.SendGraphQLQueryAsync<PlayerResponseType>(query, new { steamAccountId });

            if (playerData == null)
            {
                _logger.LogError("No se extrajo la data correctamente");
                return;
            }

            Player player = Map(playerData.Player);

            await AddPlayerAsync(player);
        }


        public Player Map(PlayerDto playerDto)
        {
            return new Player
            {
                Id = playerDto.SteamAccountId,
                Name = playerDto.SteamAccount.ProSteamAccount.Name,
                RealName = playerDto.SteamAccount.ProSteamAccount.RealName,
                TeamId = playerDto.SteamAccount.ProSteamAccount.TeamId,
                IsLocked = playerDto.SteamAccount.ProSteamAccount.IsLocked,
                IsPro = playerDto.SteamAccount.ProSteamAccount.IsPro,
                TotalEarnings = playerDto.SteamAccount.ProSteamAccount.TotalEarnings,
                Birthday = Utils.ConvertUnixToDateTime(playerDto.SteamAccount.ProSteamAccount.Birthday),
                Position = playerDto.SteamAccount.ProSteamAccount.Position,
                CountryCode = playerDto.SteamAccount?.CountryCode,
            };
        }

        public async Task AddPlayerAsync(Player player)
        {
            _context.Player.Add(player);
            await _context.SaveChangesAsync();
        }

        public bool GetPlayer(long playerId)
        {
            return _context.Player.
        }
    }
}
