using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories;

public class MatchRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<MatchRepository> _logger;
    private readonly GraphQLService _graphQLService;
    private readonly MatchStatsPickBansRepository _matchStatsPickBansRepository;
    private readonly MatchPlayerRepository _matchPlayerRepository;

    public MatchRepository(AppDbContext context, ILogger<MatchRepository> logger, GraphQLService graphQLService, 
        MatchStatsPickBansRepository matchStatsPickBansRepository, MatchPlayerRepository matchPlayerRepository)
    {
        _context = context;
        _logger = logger;
        _graphQLService = graphQLService;
        _matchStatsPickBansRepository = matchStatsPickBansRepository;
        _matchPlayerRepository = matchPlayerRepository;
    }

    public async Task GetOrFetchMatch(long matchId)
    {
        _logger.LogInformation("Buscando partida con id: {matchId}", matchId);
        Match? match = await _context.Match.FindAsync(matchId);

        if (match == null)
        {
            _logger.LogInformation("No se encuentra el id de la partida, ingresando partida");
            MatchDto matchDto = await GetMatch(matchId);
            await ProcessMatchData(matchDto);
        }
        else
        {
            _logger.LogInformation("Se encontro la partida");
            return;
        }
    }

    public async Task<MatchDto> GetMatch(long matchId)
    {
        const string query = @"
        query($matchId: Long!) {
            match(id: $matchId) {
                id
                didRadiantWin
                durationSeconds
                startDateTime
                endDateTime
                firstBloodTime
                leagueId
                radiantTeamId
                direTeamId
                gameVersionId
                winRates
                predictedWinRates
                radiantKills
                direKills
                radiantNetworthLeads
                radiantExperienceLeads
                pickBans {
                    isPick
                    heroId
                    order
                    isRadiant
                    isCaptain
                    playerIndex
                }
                players {
                    steamAccountId
                    isRadiant
                    isVictory
                    heroId
                    kills
                    deaths
                    assists
                    numLastHits
                    numDenies
                    goldPerMinute
                    networth
                    experiencePerMinute
                    level
                    gold
                    goldSpent
                    heroDamage
                    towerDamage
                    heroHealing
                    lane
                    position
                    role
                    roleBasic
                    award
                    item0Id
                    item1Id
                    item2Id
                    item3Id
                    item4Id
                    item5Id
                    backpack0Id
                    backpack1Id
                    backpack2Id
                    neutral0Id
                }
            }
        }";

        _logger.LogInformation("Extrayendo partida con id: {matchId}", matchId);

        var matchResponseType = await _graphQLService.SendGraphQLQueryAsync<MatchResponseType>(query, new { matchId });

        if (matchResponseType == null)
        {
            throw new Exception("No se extrajo la data correctamente");
        }

        _logger.LogInformation("Se extrajo la partida correctamente correctamente");

        return matchResponseType.match;
    }

    public async Task ProcessMatchData(MatchDto matchDto)
    {
        Match match = Map(matchDto);

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _logger.LogInformation("Ingresando data de la partida a la base de datos");
            await _context.Match.AddAsync(match);
            _logger.LogInformation("Ingreso correcto");
            _logger.LogInformation("Ingresando data de los picks y bans a la base de datos");
            await _matchStatsPickBansRepository.ProcessMatchPickBan(matchDto.PickBans, match.Id);
            _logger.LogInformation("Ingreso Correcto");
            _logger.LogInformation("Ingresando data de los jugadores a la base de datos");
            await _matchPlayerRepository.ProcessMatchPlayerData(matchDto.Players, match.Id);
            _logger.LogInformation("Ingreso correcto");

            _logger.LogInformation("WinRates Count: {count}", matchDto.WinRates.Count);
            _logger.LogInformation("PredictedWinRates Count: {count}", matchDto.PredictedWinRates.Count);
            _logger.LogInformation("RadiantKills Count: {count}", matchDto.RadiantKills.Count);
            _logger.LogInformation("DireKills Count: {count}", matchDto.DireKills.Count);
            _logger.LogInformation("RadiantNetworthLeads Count: {count}", matchDto.RadiantNetworthLeads.Count);
            _logger.LogInformation("RadiantExperienceLeads Count: {count}", matchDto.RadiantExperienceLeads.Count);

            _logger.LogInformation("Añadiendo los valores minimos");


            int minCount = new List<int>
            {
                matchDto.WinRates.Count,
                matchDto.PredictedWinRates.Count,
                matchDto.RadiantKills.Count,
                matchDto.DireKills.Count,
                matchDto.RadiantNetworthLeads.Count,
                matchDto.RadiantExperienceLeads.Count
            }.Min();

            _logger.LogInformation("Cantidad de elementos procesados: {minCount}", minCount);

            for (int i = 0; i < minCount; i++)
            {
                MatchStats matchStats = MapMatchStats(
                                                    matchId: match.Id,
                                                    min: i,
                                                    winRate: matchDto.WinRates.ElementAt(i),
                                                    predictedWinRate: matchDto.PredictedWinRates.ElementAt(i),
                                                    radiantKills: matchDto.RadiantKills.ElementAt(i),
                                                    direKills: matchDto.DireKills.ElementAt(i),
                                                    radiantNetworthLead: matchDto.RadiantNetworthLeads.ElementAt(i),
                                                    radiantExperienceLead: matchDto.RadiantExperienceLeads.ElementAt(i)
                                                );

                await _context.MatchStats.AddAsync(matchStats);
                _logger.LogInformation("Guardado matchStats con ID {matchId} y minuto {min}", match.Id, i);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error en alguna parte del ingreso de datos: {ex.Message}", ex.Message);
            await transaction.RollbackAsync();
        }
    }

    public Match Map(MatchDto matchDto)
    {
        return new Match
        {
            Id = matchDto.Id,
            DidRadiantWin = matchDto.DidRadiantWin,
            DurationSeconds = matchDto.DurationSeconds,
            StartDateTime = Utils.ConvertUnixToDateTime(matchDto.StartDateTime),
            EndDateTime = Utils.ConvertUnixToDateTime(matchDto.EndDateTime),
            FirstBloodTime = matchDto.FirstBloodTime,
            RadiantTeamId = matchDto.RadiantTeamId,
            DireTeamId = matchDto.DireTeamId,
            GameVersionId = matchDto.GameVersionId,
        };
    }

    public MatchStats MapMatchStats(long matchId, int min, decimal winRate, decimal predictedWinRate, 
                        int radiantKills, int direKills, int radiantNetworthLead, int radiantExperienceLead)
    {
        return new MatchStats
        {
            MatchId = matchId,
            Min = min,
            WinRate = winRate,
            PredictedWinRate = predictedWinRate,
            RadiantKills = radiantKills,
            DireKills = direKills,
            RadiantNetworthLead = radiantNetworthLead,
            RadiantExperienceLead = radiantExperienceLead
        };
    }
}
