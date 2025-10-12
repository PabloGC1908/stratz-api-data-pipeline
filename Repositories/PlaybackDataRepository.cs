using Azure.Core;
using Microsoft.EntityFrameworkCore;
using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.DTOs.Match.Events;
using StratzAPI.Models;
using StratzAPI.Models.Events;
using StratzAPI.Services;

namespace StratzAPI.Repositories
{
    public class PlaybackDataRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlaybackDataRepository> _logger;
        private readonly GraphQLService _graphQLService;

        public PlaybackDataRepository(AppDbContext context, ILogger<PlaybackDataRepository> logger,
            GraphQLService graphQLService)
        {
            _context = context;
            _logger = logger;
            _graphQLService = graphQLService;
        }

        public async Task ProcessPlaybackMatchPlayerData(PlaybackDataDto playbackData, long matchPlayerId)
        {
            if (playbackData == null)
            {
                _logger.LogWarning("PlaybackData es nulo, se omite procesamiento");
                return;
            }

            await AddEvents(playbackData.AbilityLearnEvents, mapAbilityLearnEvent, _context.AbilityLearnEvent, matchPlayerId);
            await AddEvents(playbackData.AbilityUsedEvents, mapAbilityUsedEvent, _context.AbilityUsedEvent, matchPlayerId);
            await AddEvents(playbackData.AssistEvents, mapAssistEvent, _context.AssistEvent, matchPlayerId);
            await AddEvents(playbackData.BuyBackEvents, mapBuyBackEvent, _context.BuyBackEvent, matchPlayerId);
            await AddEvents(playbackData.CsEvents, mapCsEvent, _context.CsEvent, matchPlayerId);
            await AddEvents(playbackData.ExperienceEvents, mapExperienceEvent, _context.ExperienceEvent, matchPlayerId);
            await AddEvents(playbackData.GoldEvents, mapGoldEvent, _context.GoldEvent, matchPlayerId);
            await AddEvents(playbackData.HealEvents, mapHealEvent, _context.HealEvent, matchPlayerId);
            await AddEvents(playbackData.HeroDamageEvents, mapHeroDamageEvent, _context.HeroDamageEvent, matchPlayerId);


            if (playbackData.InventoryEvents == null)
            {
                _logger.LogWarning("InventoryEvents llega como nulo, omitiendo procesamiento");
                return;
            } else
            {
                foreach (var inventoryEventDto in playbackData.InventoryEvents)
                {
                    if (inventoryEventDto != null)
                    {
                        InventoryEvent inventoryEvent = mapInventoryEvent(inventoryEventDto, matchPlayerId);
                        await _context.InventoryEvent.AddAsync(inventoryEvent);
                    }
                }
            }


            await AddEvents(playbackData.KillEvents, mapKillEvent, _context.KillEvent, matchPlayerId);
            await AddEvents(playbackData.PlayerUpdateAttributeEvents, mapPlayerUpdateAttributeEvent, _context.PlayerUpdateAttributeEvent, matchPlayerId);
            await AddEvents(playbackData.PlayerUpdateBattleEvents, mapPlayerUpdateBattleEvent, _context.PlayerUpdateBattleEvent, matchPlayerId);
            await AddEvents(playbackData.PlayerUpdateGoldEvents, mapPlayerUpdateGoldEvent, _context.PlayerUpdateGoldEvent, matchPlayerId);
            await AddEvents(playbackData.PlayerUpdateHealthEvents, mapPlayerUpdateHealthEvent, _context.PlayerUpdateHealthEvent, matchPlayerId);
            await AddEvents(playbackData.PlayerUpdateLevelEvents, mapPlayerUpdateLevelEvent, _context.PlayerUpdateLevelEvent, matchPlayerId);
            await AddEvents(playbackData.PlayerUpdatePositionEvents, mapPlayerUpdatePositionEvent, _context.PlayerUpdatePositionEvent, matchPlayerId);
            await AddEvents(playbackData.PurchaseEvents, mapPurchaseEvent, _context.PurchaseEvent, matchPlayerId);
            await AddEvents(playbackData.RuneEvents, mapRuneEvent, _context.RuneEvent, matchPlayerId);

            if (playbackData.SpiritBearInventoryEvents == null)
            {
                _logger.LogWarning("SpiritBearInventoryEvents llega como nulo, omitiendo procesamiento");
                return;
            }
            else
            {
                foreach (var spiritBearInventoryEventDto in playbackData.SpiritBearInventoryEvents)
                {
                    if (spiritBearInventoryEventDto != null)
                    {
                        SpiritBearInventoryEvent spiritBearInventoryEvent = mapSpiritBearInventoryEvent(spiritBearInventoryEventDto, matchPlayerId);
                        await _context.SpiritBearInventoryEvent.AddAsync(spiritBearInventoryEvent);
                    }
                }
            }

            await AddEvents(playbackData.StreakEvents, mapStreakEvent, _context.StreakEvent, matchPlayerId);
            await AddEvents(playbackData.TowerDamageEvents, mapTowerDamageEvent, _context.TowerDamageEvent, matchPlayerId);
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

        public AbilityLearnEvent mapAbilityLearnEvent(AbilityLearnEventDto abilityLearnEventDto, long matchPlayerId)
        {
            return new AbilityLearnEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = abilityLearnEventDto.Time,
                AbilityId = abilityLearnEventDto.AbilityId,
                LevelObtained = abilityLearnEventDto.LevelObtained,
                Level = abilityLearnEventDto.Level,
                IsUltimate = abilityLearnEventDto.IsUltimate,
                IsTalent = abilityLearnEventDto.IsTalent,
                IsMaxLevel = abilityLearnEventDto.IsMaxLevel
            };
        }

        public AbilityUsedEvent mapAbilityUsedEvent(AbilityUsedEventDto abilityUsedEventDto, long matchPlayerId)
        {
            return new AbilityUsedEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = abilityUsedEventDto.Time,
                AbilityId = abilityUsedEventDto.AbilityId,
                Attacker = abilityUsedEventDto.Attacker,
                Target = abilityUsedEventDto.Target
            };
        }

        public AssistEvent mapAssistEvent(AssistEventDto assistEventDto, long matchPlayerId)
        {
            return new AssistEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = assistEventDto.Time,
                Attacker = assistEventDto.Attacker,
                Target = assistEventDto.Target,
                Gold = assistEventDto.Gold,
                Xp = assistEventDto.Xp,
                SubTime = assistEventDto.SubTime,
                PositionX = assistEventDto.PositionX,
                PositionY = assistEventDto.PositionY
            };
        }

        public BuyBackEvent mapBuyBackEvent(BuyBackEventDto buyBackEventDto, long matchPlayerId)
        {
            return new BuyBackEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = buyBackEventDto.Time,
                HeroId = buyBackEventDto.HeroId,
                DeathTimeRemaining = buyBackEventDto.DeathTimeRemaining,
                Cost = buyBackEventDto.Cost
            };
        }

        public CsEvent mapCsEvent(CsEventDto csEventDto, long matchPlayerId)
        {
            return new CsEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = csEventDto.Time,
                Attacker = csEventDto.Attacker,
                IsFromIllusion = csEventDto.IsFromIllusion,
                NpcId = csEventDto.NpcId,
                ByAbility = csEventDto.ByAbility,
                ByItem = csEventDto.ByItem,
                Gold = csEventDto.Gold,
                Xp = csEventDto.Xp,
                PositionX = csEventDto.PositionX,
                PositionY = csEventDto.PositionY,
                IsCreep = csEventDto.IsCreep,
                IsNeutral = csEventDto.IsNeutral,
                IsAncient = csEventDto.IsAncient,
                MapLocation = csEventDto.MapLocation
            };
        }

        public ExperienceEvent mapExperienceEvent(ExperienceEventDto experienceEventDto, long matchPlayerId)
        {
            return new ExperienceEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = experienceEventDto.Time,
                Amount = experienceEventDto.Amount,
                PositionX = experienceEventDto.PositionX,
                PositionY = experienceEventDto.PositionY
            };
        }

        public GoldEvent mapGoldEvent(GoldEventDto goldEventDto, long matchPlayerId)
        {
            return new GoldEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = goldEventDto.Time,
                Amount = goldEventDto.Amount,
                Reason = goldEventDto.Reason,
                NpcId = goldEventDto.NpcId,
                IsValidForStats = goldEventDto.IsValidForStats
            };
        }

        public HealEvent mapHealEvent(HealEventDto healEventDto, long matchPlayerId)
        {
            return new HealEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = healEventDto.Time,
                Attacker = healEventDto.Attacker,
                Target = healEventDto.Target,
                Value = healEventDto.Value,
                ByAbility = healEventDto.ByAbility,
                ByItem = healEventDto.ByItem
            };
        }

        public HeroDamageEvent mapHeroDamageEvent(HeroDamageEventDto heroDamageEventDto, long matchPlayerId)
        {
            return new HeroDamageEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = heroDamageEventDto.Time,
                Attacker = heroDamageEventDto.Attacker,
                Target = heroDamageEventDto.Target,
                Value = heroDamageEventDto.Value,
                ByAbility = heroDamageEventDto.ByAbility,
                ByItem = heroDamageEventDto.ByItem,
                DamageType = heroDamageEventDto.DamageType,
                FromNpc = heroDamageEventDto.FromNpc,
                ToNpc = heroDamageEventDto.ToNpc,
                FromIllusion = heroDamageEventDto.FromIllusion,
                ToIllusion = heroDamageEventDto.ToIllusion,
                IsPhysicalAttack = heroDamageEventDto.IsPhysicalAttack,
                IsSourceMainHero = heroDamageEventDto.IsSourceMainHero,
                IsTargetMainHero = heroDamageEventDto.IsTargetMainHero
            };
        }

        public InventoryEvent mapInventoryEvent(InventoryEventDto dto, long matchPlayerId)
        {
            return new InventoryEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = dto.Time,

                Item0 = dto.Item0?.ItemId,
                Item0Charges = dto.Item0?.Charges,
                Item1 = dto.Item1?.ItemId,
                Item1Charges = dto.Item1?.Charges,
                Item2 = dto.Item2?.ItemId,
                Item2Charges = dto.Item2?.Charges,
                Item3 = dto.Item3?.ItemId,
                Item3Charges = dto.Item3?.Charges,
                Item4 = dto.Item4?.ItemId,
                Item4Charges = dto.Item4?.Charges,
                Item5 = dto.Item5?.ItemId,
                Item5Charges = dto.Item5?.Charges,

                BackPack0 = dto.BackPack0?.ItemId,
                BackPack0Charges = dto.BackPack0?.Charges,
                BackPack1 = dto.BackPack1?.ItemId,
                BackPack1Charges = dto.BackPack1?.Charges,
                BackPack2 = dto.BackPack2?.ItemId,
                BackPack2Charges = dto.BackPack2?.Charges,

                Teleport0 = dto.Teleport0?.ItemId,
                Teleport0Charges = dto.Teleport0?.Charges,

                Neutral0 = dto.Neutral0?.ItemId,
                Neutral0Charges = dto.Neutral0?.Charges
            };
        }

        public KillEvent mapKillEvent(KillEventDto killEventDto, long matchPlayerId)
        {
            return new KillEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = killEventDto.Time,
                Attacker = killEventDto.Attacker,
                IsFromIllusion = killEventDto.IsFromIllusion,
                Target = killEventDto.Target,
                ByAbility = killEventDto.ByAbility,
                ByItem = killEventDto.ByItem,
                Gold = killEventDto.Gold,
                Xp = killEventDto.Xp,
                PositionX = killEventDto.PositionX,
                PositionY = killEventDto.PositionY,
                IsSolo = killEventDto.IsSolo,
                IsGank = killEventDto.IsGank,
                IsInvisible = killEventDto.IsInvisible,
                IsSmoke = killEventDto.IsSmoke,
                IsTpRecently = killEventDto.IsTpRecently,
                IsRuneEffected = killEventDto.IsRuneEffected
            };
        }

        public PlayerUpdateAttributeEvent mapPlayerUpdateAttributeEvent(
                            PlayerUpdateAttributeEventDto playerUpdateAttributeDto, long matchPlayerId)
        {
            return new PlayerUpdateAttributeEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerUpdateAttributeDto.Time,
                Agi = playerUpdateAttributeDto.Agi,
                Int = playerUpdateAttributeDto.Int,
                Str = playerUpdateAttributeDto.Str
            };
        }

        public PlayerUpdateBattleEvent mapPlayerUpdateBattleEvent(
                            PlayerUpdateBattleEventDto playerUpdateBattleEventDto, long matchPlayerId)
        {
            return new PlayerUpdateBattleEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerUpdateBattleEventDto.Time,
                DamageMinMax = playerUpdateBattleEventDto.DamageMinMax,
                DamageBonus = playerUpdateBattleEventDto.DamageBonus,
                HpRegen = playerUpdateBattleEventDto.HpRegen,
                MpRegen = playerUpdateBattleEventDto.MpRegen
            };
        }

        public PlayerUpdateGoldEvent mapPlayerUpdateGoldEvent(
                            PlayerUpdateGoldEventDto playerUpdateGoldEventDto, long matchPlayerId)
        {
            return new PlayerUpdateGoldEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerUpdateGoldEventDto.Time,
                Gold = playerUpdateGoldEventDto.Gold,
                UnreliableGold = playerUpdateGoldEventDto.UnreliableGold,
                Networth = playerUpdateGoldEventDto.Networth,
                NetworthDifference = playerUpdateGoldEventDto.NetworthDifference
            };
        }

        public PlayerUpdateHealthEvent mapPlayerUpdateHealthEvent(
                            PlayerUpdateHealthEventDto playerUpdateHealthEventDto, long matchPlayerId)
        {
            return new PlayerUpdateHealthEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerUpdateHealthEventDto.Time,
                Hp = playerUpdateHealthEventDto.Hp,
                MaxHp = playerUpdateHealthEventDto.MaxHp,
                Mp = playerUpdateHealthEventDto.Mp,
                MaxMp = playerUpdateHealthEventDto.MaxMp
            };
        }

        public PlayerUpdateLevelEvent mapPlayerUpdateLevelEvent(
                            PlayerUpdateLevelEventDto playerUpdateLevelEventDto, long matchPlayerId)
        {
            return new PlayerUpdateLevelEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerUpdateLevelEventDto.Time,
                Level = playerUpdateLevelEventDto.Level
            };
        }

        public PlayerUpdatePositionEvent mapPlayerUpdatePositionEvent(
                            PlayerUpdatePositionEventDto playerUpdatePositionEventDto, long matchPlayerId)
        {
            return new PlayerUpdatePositionEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerUpdatePositionEventDto.Time,
                X = playerUpdatePositionEventDto.X,
                Y = playerUpdatePositionEventDto.Y,
            };
        }

        public PurchaseEvent mapPurchaseEvent(PurchaseEventDto playerPurchaseEventDto, long matchPlayerId)
        {
            return new PurchaseEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = playerPurchaseEventDto.Time,
                ItemId = playerPurchaseEventDto.ItemId
            };
        }

        public RuneEvent mapRuneEvent(RuneEventDto runeEventDto, long matchPlayerId)
        {
            return new RuneEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = runeEventDto.Time,
                Rune = runeEventDto.Rune,
                Action = runeEventDto.Action,
                Gold = runeEventDto.Gold,
                PositionX = runeEventDto.PositionX,
                PositionY = runeEventDto.PositionY
            };
        }

        public SpiritBearInventoryEvent mapSpiritBearInventoryEvent(
                            SpiritBearInventoryEventDto spiritBearInventoryEventDto, long matchPlayerId)
        {
            return new SpiritBearInventoryEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = spiritBearInventoryEventDto.Time,
                Item0 = spiritBearInventoryEventDto?.Item0?.ItemId,
                Item1 = spiritBearInventoryEventDto?.Item1?.ItemId,
                Item2 = spiritBearInventoryEventDto?.Item2?.ItemId,
                Item3 = spiritBearInventoryEventDto?.Item3?.ItemId,
                Item4 = spiritBearInventoryEventDto?.Item4?.ItemId,
                Item5 = spiritBearInventoryEventDto?.Item5?.ItemId,
                BackPack0 = spiritBearInventoryEventDto?.BackPack0?.ItemId,
                BackPack1 = spiritBearInventoryEventDto?.BackPack1?.ItemId,
                BackPack2 = spiritBearInventoryEventDto?.BackPack2?.ItemId,
                Neutral0 = spiritBearInventoryEventDto?.Neutral0?.ItemId,
                Teleport0 = spiritBearInventoryEventDto?.Neutral0?.ItemId
            };
        }

        public StreakEvent mapStreakEvent(StreakEventDto streakEventDto, long matchPlayerId)
        {
            return new StreakEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = streakEventDto.Time,
                HeroId = streakEventDto.HeroId,
                Type = streakEventDto.Type,
                Value = streakEventDto.Value
            };
        }

        public TowerDamageEvent mapTowerDamageEvent(TowerDamageEventDto towerDamageEventDto, long matchPlayerId)
        {
            return new TowerDamageEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = towerDamageEventDto.Time,
                Attacker = towerDamageEventDto.Attacker,
                NpcId = towerDamageEventDto.NpcId,
                Damage = towerDamageEventDto.Damage,
                ByAbility = towerDamageEventDto.ByAbility,
                ByItem = towerDamageEventDto.ByItem,
                FromNpc = towerDamageEventDto.FromNpc
            };
        }
    }
}
