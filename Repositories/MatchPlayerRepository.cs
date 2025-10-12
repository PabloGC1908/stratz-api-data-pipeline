using Microsoft.EntityFrameworkCore;
using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.Models;
using System.Numerics;

namespace StratzAPI.Repositories
{
    public class MatchPlayerRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MatchPlayerRepository> _logger;
        private readonly PlaybackDataRepository _playbackDataRepository;
        private readonly PlayerRepository _playerRepository;

        public MatchPlayerRepository(AppDbContext context, ILogger<MatchPlayerRepository> logger,
                                    PlaybackDataRepository playbackDataRepository, PlayerRepository playerRepository)
        {
            _context = context;
            _logger = logger;
            _playbackDataRepository = playbackDataRepository;
            _playerRepository = playerRepository;
        }

        public async Task ProcessMatchPlayerData(ICollection<MatchPlayerDto> matchPlayersDto, long matchId)
        {
            _logger.LogInformation("Iniciando procesamiento de jugadores para Match {MatchId}", matchId);

            foreach (var matchPlayerDto in matchPlayersDto)
            {
                bool playerExist = await _context.Player
                                .AsNoTracking()
                                .AnyAsync(player => player.Id == matchPlayerDto.SteamAccountId);

                if (!playerExist)
                {
                    Player? newPlayer = await _playerRepository.FetchAndSavePlayer(matchPlayerDto.SteamAccountId);
                    await _context.Player.AddAsync(newPlayer);
                    await _context.SaveChangesAsync();
                }

                MatchPlayer matchPlayer = MatchPlayerDtoToMatchPlayer(matchPlayerDto, matchId);
                await _context.MatchPlayer.AddAsync(matchPlayer);
            }

            await _context.SaveChangesAsync();

            foreach (var matchPlayerDto in matchPlayersDto)
            {
                MatchPlayer? matchPlayerDb = _context.MatchPlayer
                                .AsNoTracking()
                                .Where(mP => mP.MatchId == matchId && mP.PlayerId == matchPlayerDto.SteamAccountId)
                                .FirstOrDefault() ?? throw new Exception("No se guardo la data de la partida del jugador");

                _logger.LogInformation("Se encontro el MatchPlayerId, ingresando los items");

                MatchPlayerItems matchPlayerItems = MatchPlayerDtoToMatchPlayerItems(matchPlayerDto, matchPlayerDb.Id);
                await _playbackDataRepository.ProcessPlaybackMatchPlayerData(matchPlayerDto.PlaybackData, matchPlayerDb.Id);


                await _context.MatchPlayerItems.AddAsync(matchPlayerItems);
            }

            await _context.SaveChangesAsync();
        }

        public MatchPlayer MatchPlayerDtoToMatchPlayer(MatchPlayerDto matchPlayerDto, long matchId)
        {
            return new MatchPlayer
            {
                MatchId = matchId,
                PlayerId = matchPlayerDto.SteamAccountId,
                IsRadiant = matchPlayerDto.IsRadiant,
                HeroId = matchPlayerDto.HeroId,
                Kills = matchPlayerDto.Kills,
                Deaths = matchPlayerDto.Deaths,
                Assists = matchPlayerDto.Assists,
                NumLastHits = matchPlayerDto.NumLastHits,
                NumDenies = matchPlayerDto.NumDenies,
                GoldPerMinute = matchPlayerDto.GoldPerMinute,
                Networth = matchPlayerDto.Networth,
                ExperiencePerMinute = matchPlayerDto.ExperiencePerMinute,
                Level = matchPlayerDto.Level,
                Gold = matchPlayerDto.Gold,
                GoldSpent = matchPlayerDto.GoldSpent,
                HeroDamage = matchPlayerDto.HeroDamage,
                TowerDamage = matchPlayerDto.TowerDamage,
                HeroHealing = matchPlayerDto.HeroHealing,
                Lane = matchPlayerDto.Lane,
                Position = matchPlayerDto.Position,
                Role = matchPlayerDto.Rol,
                RoleBasic = matchPlayerDto.RolBasic,
                Award = matchPlayerDto.Award,
            };
        }

        public MatchPlayerItems MatchPlayerDtoToMatchPlayerItems(MatchPlayerDto matchPlayerDto, long id)
        {
            return new MatchPlayerItems
            {
                MatchPlayerId = id,
                Item0Id = matchPlayerDto.Item0Id,
                Item1Id = matchPlayerDto.Item1Id,
                Item2Id = matchPlayerDto.Item2Id,
                Item3Id = matchPlayerDto.Item3Id,
                Item4Id = matchPlayerDto.Item4Id,
                Item5Id = matchPlayerDto.Item5Id,
                Backpack0Id = matchPlayerDto.Backpack0Id,
                Backpack1Id = matchPlayerDto.Backpack1Id,
                Backpack2Id = matchPlayerDto.Backpack2Id,
                Neutral0Id = matchPlayerDto.Neutral0Id
            };
        }
    }
}
