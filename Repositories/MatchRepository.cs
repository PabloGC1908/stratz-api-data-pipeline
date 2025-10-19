using Microsoft.EntityFrameworkCore;
using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.DTOs.Match.MatchEvents;
using StratzAPI.Models;
using StratzAPI.Models.MatchEvents;
using StratzAPI.Services;
using System.Text.Json;

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

    public async Task GetOrUpdateMatch(long matchId)
    {
        _logger.LogInformation("Actualizando partida con id: {matchId}", matchId);
        Match? match = await _context.Match.FindAsync(matchId);

        if (match == null)
        {
            _logger.LogInformation("No se encuentra el id de la partida, registrando partida");
            MatchDto matchDto = await GetMatch(matchId);
            await ProcessMatchData(matchDto);
        }
        else
        {
            _logger.LogInformation("Se encontro la partida, actualizando partida");
            MatchDto matchUpdateDto = await GetMatch(matchId);

            await ProcessMatchData(matchUpdateDto);
        }
    }

    public async Task<MatchUpdateDto> UpdateMatch(long matchId)
    {
        const string query = @"
        query($matchId: Long!) {
            match(id: $matchId) {
                id
                playbackData {
                  courierEvents {
                    id
                    ownerHero
                    isRadiant
                    events {
                      time
                      positionX
                      positionY
                      hp
                      isFlying
                      respawnTime
                      didCastBoost
                      item0Id
                      item1Id
                      item2Id
                      item3Id
                      item4Id
                      item5Id
                    }
                  }
                  runeEvents {
                    indexId
                    time
                    positionX
                    positionY
                    location
                    rune
                    action
                  }
      
                  wardEvents {
                    indexId
                    time
                    positionX
                    positionY
                    fromPlayer
                    wardType
                    action
                    playerDestroyed
                  }
      
                  buildingEvents {
                    time
                    indexId
                    type
                    hp
                    maxHp
                    positionX
                    positionY
                    isRadiant
                    npcId
                    didShrineActivate
                  }
      
                  towerDeathEvents {
                    time
                    radiant
                    dire
                  }
      
                  roshanEvents {
                    time
                    hp
                    maxHp
                    createTime
                    x
                    y
                    totalDamageTaken
                    item0
                    item1
                    item2
                    item3
                    item4
                    item5
                  }
                }
            }
        }";

        _logger.LogInformation("Extrayendo partida con id: {matchId}", matchId);

        var matchResponseType = await _graphQLService.SendGraphQLQueryAsync<MatchUpdateResponseType>(query, new { matchId });

        if (matchResponseType == null)
        {
            throw new Exception("No se extrajo la data correctamente");
        }

        //_logger.LogInformation("Se extrajo la partida correctamente correctamente:\n{matchResponseType}",
        //                        JsonSerializer.Serialize(matchResponseType, new JsonSerializerOptions { WriteIndented = true }));

        return matchResponseType.match;
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

                playbackData {
                  courierEvents {
                    id
                    ownerHero
                    isRadiant
                    events {
                      time
                      positionX
                      positionY
                      hp
                      isFlying
                      respawnTime
                      didCastBoost
                      item0Id
                      item1Id
                      item2Id
                      item3Id
                      item4Id
                      item5Id
                    }
                  }
                  runeEvents {
                    indexId
                    time
                    positionX
                    positionY
                    location
                    rune
                    action
                  }
      
                  wardEvents {
                    indexId
                    time
                    positionX
                    positionY
                    fromPlayer
                    wardType
                    action
                    playerDestroyed
                  }
      
                  buildingEvents {
                    time
                    indexId
                    type
                    hp
                    maxHp
                    positionX
                    positionY
                    isRadiant
                    npcId
                    didShrineActivate
                  }
      
                  towerDeathEvents {
                    time
                    radiant
                    dire
                  }
      
                  roshanEvents {
                    time
                    hp
                    maxHp
                    createTime
                    x
                    y
                    totalDamageTaken
                    item0
                    item1
                    item2
                    item3
                    item4
                    item5
                  }
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

        //_logger.LogInformation("Se extrajo la partida correctamente correctamente:\n{matchResponseType}",
        //                        JsonSerializer.Serialize(matchResponseType, new JsonSerializerOptions { WriteIndented = true }));

        return matchResponseType.match;
    }

    public async Task ProcessUpdateMatchData(MatchUpdateDto matchDto)
    {
        if (matchDto == null)
        {
            _logger.LogError("MatchUpdateDto es nulo, omitiendo");
            return;
        }


        try
        {
            if (matchDto.PlaybackData == null)
            {
                _logger.LogError("Match Playback Data es nulo, omitiendo");
                return;
            }

            _logger.LogInformation("Actualizando data de la partida a la base de datos");
            _logger.LogInformation("Match completo:\n{match}",
                            JsonSerializer.Serialize(matchDto, new JsonSerializerOptions { WriteIndented = true }));

            await AddEvents(matchDto.PlaybackData.BuildingEvents, mapBuildingEvent, _context.BuildingEvent, matchDto.Id);
            await AddEvents(matchDto.PlaybackData.RoshanEvents, mapRoshanEvent, _context.RoshanEvent, matchDto.Id);
            await AddEvents(matchDto.PlaybackData.TowerDeathEvents, mapTowerDeathEvent, _context.TowerDeathEvent, matchDto.Id);
            await AddEvents(matchDto.PlaybackData.RuneEvents, mapMatchRuneEvent, _context.MatchRuneEvent, matchDto.Id);
            await AddEvents(matchDto.PlaybackData.WardEvents, mapWardEvent, _context.WardEvent, matchDto.Id);

            foreach (var courierEvent in matchDto.PlaybackData.CourierEvents)
            {

                foreach (var cEvent in courierEvent.Events)
                {
                    CourierEvent courierEventDb = new CourierEvent
                    {
                        MatchId = matchDto.Id,
                        OwnerHero = courierEvent.OwnerHero,
                        IsRadiant = courierEvent.IsRadiant,
                        Time = cEvent.Time,
                        PositionX = cEvent.PositionX,
                        PositionY = cEvent.PositionY,
                        Hp = cEvent.Hp,
                        IsFlying = cEvent.IsFlying,
                        RespawnTime = cEvent.RespawnTime,
                        DidCastBoost = cEvent.DidCastBoost,
                        Item0Id = cEvent.Item0Id,
                        Item1Id = cEvent.Item1Id,
                        Item2Id = cEvent.Item2Id,
                        Item3Id = cEvent.Item3Id,
                        Item4Id = cEvent.Item4Id,
                        Item5Id = cEvent.Item5Id
                    };

                    await _context.AddAsync(courierEventDb);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Guardado de la partida correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar datos del match {matchId}", matchDto.Id);
            _logger.LogError("Haciendo rollback");
            throw;
        }
    }

    public async Task ProcessMatchData(MatchDto matchDto)
    {
        
        try
        {
            _logger.LogInformation("Procesando partida {matchId}", matchDto.Id);

            Match match = Map(matchDto);

            _logger.LogInformation("Match completo:\n{match}", 
                            JsonSerializer.Serialize(match, new JsonSerializerOptions { WriteIndented = true }));

            Match? existingMatch = await _context.Match
                                    .Where(m => m.Id == match.Id)
                                    .FirstOrDefaultAsync() ?? throw new Exception("No se encontro la partida cargada a la BBDD");


            if (existingMatch == null)
            {
                _logger.LogInformation("La partida no existe, se creará una nueva.");
                await _context.Match.AddAsync(match);
            } 
            else
            {
                _logger.LogInformation("La partida ya existe, se actualizarán los datos.");

                _context.Entry(existingMatch).CurrentValues.SetValues(match);
            }

            await _context.SaveChangesAsync();

            var matchDb = await _context.Match.FirstOrDefaultAsync(m => m.Id == match.Id);
            if (matchDb == null)
                throw new Exception("No se encontró la partida guardada en la base de datos.");

            _logger.LogInformation("Partida guardada/actualizada correctamente con ID {Id}", matchDb.Id);

            await _matchStatsPickBansRepository.ProcessMatchPickBan(matchDto.PickBans, matchDb.Id);

            await _matchPlayerRepository.ProcessMatchPlayerData(matchDto.Players, matchDb.Id);


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
                    matchId: existingMatch.Id,
                    min: i,
                    winRate: i < matchDto.WinRates.Count ? matchDto.WinRates.ElementAt(i) : (decimal?)null,
                    predictedWinRate: i < matchDto.PredictedWinRates.Count ? matchDto.PredictedWinRates.ElementAt(i) : (decimal?)null,
                    radiantKills: i < matchDto.RadiantKills.Count ? matchDto.RadiantKills.ElementAt(i) : (int?)null,
                    direKills: i < matchDto.DireKills.Count ? matchDto.DireKills.ElementAt(i) : (int?)null,
                    radiantNetworthLead: i < matchDto.RadiantNetworthLeads.Count ? matchDto.RadiantNetworthLeads.ElementAt(i) : (int?)null,
                    radiantExperienceLead: i < matchDto.RadiantExperienceLeads.Count ? matchDto.RadiantExperienceLeads.ElementAt(i) : (int?)null
                );

                await _context.MatchStats.AddAsync(matchStats);
                _logger.LogInformation("Guardado matchStats con ID {matchId} y minuto {min}", existingMatch.Id, i);
            }

            if (matchDto.PlaybackData == null)
            {
                _logger.LogWarning("Match PlaybackData es nulo,omitiendo");
            }
            else
            {
                await AddEvents(matchDto.PlaybackData.BuildingEvents, mapBuildingEvent, _context.BuildingEvent, matchDto.Id);
                await AddEvents(matchDto.PlaybackData.RoshanEvents, mapRoshanEvent, _context.RoshanEvent, matchDto.Id);
                await AddEvents(matchDto.PlaybackData.TowerDeathEvents, mapTowerDeathEvent, _context.TowerDeathEvent, matchDto.Id);
                await AddEvents(matchDto.PlaybackData.RuneEvents, mapMatchRuneEvent, _context.MatchRuneEvent, matchDto.Id);
                await AddEvents(matchDto.PlaybackData.WardEvents, mapWardEvent, _context.WardEvent, matchDto.Id);

                foreach (var courierEvent in matchDto.PlaybackData.CourierEvents)
                {

                    foreach (var cEvent in courierEvent.Events)
                    {
                        CourierEvent courierEventDb = new CourierEvent
                        {
                            MatchId = matchDto.Id,
                            OwnerHero = courierEvent.OwnerHero,
                            IsRadiant = courierEvent.IsRadiant,
                            Time = cEvent.Time,
                            PositionX = cEvent.PositionX,
                            PositionY = cEvent.PositionY,
                            Hp = cEvent.Hp,
                            IsFlying = cEvent.IsFlying,
                            RespawnTime = cEvent.RespawnTime,
                            DidCastBoost = cEvent.DidCastBoost,
                            Item0Id = cEvent.Item0Id,
                            Item1Id = cEvent.Item1Id,
                            Item2Id = cEvent.Item2Id,
                            Item3Id = cEvent.Item3Id,
                            Item4Id = cEvent.Item4Id,
                            Item5Id = cEvent.Item5Id
                        };

                        await _context.AddAsync(courierEventDb);
                    }
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Procesamiento de la partida {matchId} completado correctamente", matchDto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar datos del match {matchId}", matchDto.Id);
            throw;
        }
    }
    private async Task AddEvents<TDto, TEntity>(IEnumerable<TDto>? dtos, Func<TDto, long, TEntity> mapFunc,
                                     DbSet<TEntity> dbSet, long matchPlayerId)
    where TDto : class
    where TEntity : class
    {
        if (dtos == null || !dtos.Any())
            return;

        foreach (var dto in dtos)
        {
            if (dto == null) continue;
            var entity = mapFunc(dto, matchPlayerId);
            await dbSet.AddAsync(entity);
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

    public BuildingEvent mapBuildingEvent(BuildingEventDto buildingEventDto, long matchId)
    {
        return new BuildingEvent
        {
            MatchId = matchId,
            Time = buildingEventDto.Time,
            IndexId = buildingEventDto.IndexId,
            Type = buildingEventDto.Type,
            Hp = buildingEventDto.Hp,
            MaxHp = buildingEventDto.MaxHp,
            PositionX = buildingEventDto.PositionX,
            PositionY = buildingEventDto.PositionY,
            IsRadiant = buildingEventDto.IsRadiant,
            NpcId = buildingEventDto.NpcId,
            DidShrineActivate = buildingEventDto.DidShrineActivate
        };
    }

    public MatchRuneEvent mapMatchRuneEvent(MatchRuneEventDto matchRuneEventDto, long matchId)
    {
        return new MatchRuneEvent
        {
            MatchId = matchId,
            IndexId = matchRuneEventDto.IndexId,
            Time = matchRuneEventDto.Time,
            PositionX = matchRuneEventDto.PositionX,
            PositionY = matchRuneEventDto.PositionY,
            Location = matchRuneEventDto.Location,
            Rune = matchRuneEventDto.Rune,
            Action = matchRuneEventDto.Action
        };
    }

    public RoshanEvent mapRoshanEvent(RoshanEventDto roshanEventDto, long matchId)
    {
        return new RoshanEvent
        {
            MatchId = matchId,
            Time = roshanEventDto.Time,
            Hp = roshanEventDto.Hp,
            MaxHp = roshanEventDto.MaxHp,
            CreateTime = roshanEventDto.CreateTime,
            X = roshanEventDto.X,
            Y = roshanEventDto.Y,
            TotalDamageTaken = roshanEventDto.TotalDamageTaken,
            Item0 = roshanEventDto.Item0,
            Item1 = roshanEventDto.Item1,
            Item2 = roshanEventDto.Item2,
            Item3 = roshanEventDto.Item3,
            Item4 = roshanEventDto.Item4,
            Item5 = roshanEventDto.Item5
        };
    }

    public TowerDeathEvent mapTowerDeathEvent(TowerDeathEventDto towerDeathEventDto, long matchId)
    {
        return new TowerDeathEvent
        {
            MatchId = matchId,
            Time = towerDeathEventDto.Time,
            Radiant = towerDeathEventDto.Radiant,
            Dire = towerDeathEventDto.Dire,
        };
    }

    public WardEvent mapWardEvent(WardEventDto wardEventDto, long matchId)
    {
        return new WardEvent
        {
            MatchId = matchId,
            Time = wardEventDto.Time,
            IndexId = wardEventDto.IndexId,
            PositionX = wardEventDto.PositionX,
            PositionY = wardEventDto.PositionY,
            FromPlayer = wardEventDto.FromPlayer,
            WardType = wardEventDto.WardType,
            Action = wardEventDto.Action,
            PlayerDestroyed = wardEventDto.PlayerDestroyed
        };
    }
}
