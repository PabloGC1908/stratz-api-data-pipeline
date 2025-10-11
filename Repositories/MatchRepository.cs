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

                    playbackData {
                        abilityLearnEvents {
                          time
                          abilityId
                          levelObtained
                          level
                          isUltimate
                          isTalent
                          isMaxLevel
                        }
                        abilityUsedEvents{
                          time
                          abilityId
                          attacker
                          target
                        }
        
                        itemUsedEvents {
                          time
                          itemId
                          attacker
                          target
                        }
      	
                        playerUpdatePositionEvents {
                          time
                          x
                          y
                        }
        
                        playerUpdateGoldEvents {
                          time
                          gold
                          unreliableGold
                          networth
                          networthDifference
                        }
        
                        playerUpdateAttributeEvents {
                          time
                          agi
                          int
                          str
                        }
        
                        playerUpdateLevelEvents {
                          time
                          level
                        }
        
                        playerUpdateHealthEvents {
                          time
                          hp
                          maxHp
                          mp
                          maxMp
                        }
        
                        playerUpdateBattleEvents {
                          time
                          damageMinMax
                          damageBonus
                          hpRegen
                          mpRegen
                        }
        
                        killEvents {
                          time
                          attacker
                          isFromIllusion
                          target
                          byAbility
                          byItem
                          gold
                          xp
                          positionX
                          positionY
                          isSolo
                          isGank
                          isInvisible
                          isSmoke
                          isTpRecently
                          isRuneEffected
                        }
        
                        assistEvents {
                          time
                          attacker
                          target
                          gold
                          xp
                          subTime
                          positionX
                          positionY
                        }
        
                        csEvents {
                          time
                          attacker
                          isFromIllusion
                          npcId
                          byAbility
                          byItem
                          gold
                          xp
                          positionX
                          positionY
                          isCreep
                          isNeutral
                          isAncient
                          mapLocation
                        }
        
                        goldEvents {
                          time
                          amount
                          reason
                          npcId
                          isValidForStats
                        }
        
                        experienceEvents {
                          time
                          amount
                          reason
                          positionX
                          positionY
                        }
        
                        healEvents {
                          time
                          attacker
                          target
                          value
                          byAbility
                          byItem
                        }
        
                        heroDamageEvents {
                          time
                          attacker
                          target
                          value
                          byAbility
                          byItem
                          damageType
                          fromNpc
                          toNpc
                          fromIllusion
                          toIllusion
                          isPhysicalAttack
                          isSourceMainHero
                          isTargetMainHero
                        }
      	
                        towerDamageEvents {
                          time
                          attacker
                          npcId
                          damage
                          byAbility
                          byItem
                          fromNpc
                        }
        
                        inventoryEvents {
                          time
                          item0 {
                            itemId
                            charges
                            secondaryCharges
                          }
                          item1 {
                            itemId
                            charges
                            secondaryCharges
                          }
                          item2 {
                            itemId
                            charges
                            secondaryCharges
                          }
                          item3 {
                            itemId
                            charges
                            secondaryCharges
                          }
                          item4 {
                            itemId
                            charges
                            secondaryCharges
                          }
                          item5 {
                            itemId
                            charges
                            secondaryCharges
                          }
          
                          backPack0 {
                            itemId
                            charges
                            secondaryCharges
                          }
          
                          backPack1 {
                            itemId
                            charges
                            secondaryCharges
                          }
          
                          backPack2 {
                            itemId
                            charges
                            secondaryCharges
                          }
          
                          teleport0 {
                            itemId
                            charges
                            secondaryCharges
                          }
          
                          neutral0 {
                            itemId
                            charges
                            secondaryCharges
                          }
                        }
        
                        purchaseEvents {
                          time
                          itemId
                        }
        
                        buyBackEvents {
                          time
                          heroId
                          deathTimeRemaining
                          cost
                        }
                        streakEvents {
                          time
                          heroId
                          type
                          value
                        }
        
                        runeEvents {
                          time
                          rune
                          action
                          gold
                          positionX
                          positionY
                        }
        
                        spiritBearInventoryEvents {
                          time
                          item0 {
                            itemId
                          }
                          item1 {
                            itemId
                          }
                          item2 {
                            itemId
                          }
                          item3 {
                            itemId
                          }
                          item4 {
                            itemId
                          }
                          item5 {
                            itemId
                          }
          
                          backPack0 {
                            itemId
                          }
          
                          backPack1 {
                            itemId
                          }
          
                          backPack2 {
                            itemId
                          }
          
                          teleport0 {
                            itemId
                          }
          
                          neutral0 {
                            itemId
                          }
                        }
                    }
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
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            Match match = Map(matchDto);
            _logger.LogInformation("Ingresando data de la partida a la base de datos");
            await _context.Match.AddAsync(match);
            await _context.SaveChangesAsync();
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


            int maxCount = new List<int>
            {
                matchDto.WinRates.Count,
                matchDto.PredictedWinRates.Count,
                matchDto.RadiantKills.Count,
                matchDto.DireKills.Count,
                matchDto.RadiantNetworthLeads.Count,
                matchDto.RadiantExperienceLeads.Count
            }.Max();

            _logger.LogInformation("Cantidad de elementos procesados: {maxCount}", maxCount);

            for (int i = 0; i < maxCount; i++)
            {
                MatchStats matchStats = MapMatchStats(
                    matchId: match.Id,
                    min: i,
                    winRate: i < matchDto.WinRates.Count ? matchDto.WinRates.ElementAt(i) : (decimal?)null,
                    predictedWinRate: i < matchDto.PredictedWinRates.Count ? matchDto.PredictedWinRates.ElementAt(i) : (decimal?)null,
                    radiantKills: i < matchDto.RadiantKills.Count ? matchDto.RadiantKills.ElementAt(i) : (int?)null,
                    direKills: i < matchDto.DireKills.Count ? matchDto.DireKills.ElementAt(i) : (int?)null,
                    radiantNetworthLead: i < matchDto.RadiantNetworthLeads.Count ? matchDto.RadiantNetworthLeads.ElementAt(i) : (int?)null,
                    radiantExperienceLead: i < matchDto.RadiantExperienceLeads.Count ? matchDto.RadiantExperienceLeads.ElementAt(i) : (int?)null
                );

                await _context.MatchStats.AddAsync(matchStats);
                _logger.LogInformation("Guardado matchStats con ID {matchId} y minuto {min}", match.Id, i);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Guardado de la partida correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar datos del match {matchId}", matchDto.Id);
            _logger.LogError("Haciendo rollback");
            await transaction.RollbackAsync();
            throw;
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

    public MatchStats MapMatchStats(long matchId, int min, decimal? winRate, decimal? predictedWinRate, 
                        int? radiantKills, int? direKills, int? radiantNetworthLead, int? radiantExperienceLead)
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
