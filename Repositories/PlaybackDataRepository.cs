using Azure.Core;
using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.DTOs.Match.Events;
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
            foreach (var abilityLearnEventsDto in playbackData.AbilityLearnEvents)
            {

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
                Reason = experienceEventDto.Reason,
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

        public InventoryEvent mapInventoryEvent(InventoryEventDto inventoryEventDto, long matchPlayerId)
        {
            return new InventoryEvent
            {
                MatchPlayerId = matchPlayerId,
                Time = inventoryEventDto.Time,
                Item0 = inventoryEventDto.Item0.ItemId,
                Item0Charges = inventoryEventDto.Item0.Charges,
                Item1 = inventoryEventDto.Item1.ItemId,
                Item1Charges = inventoryEventDto.Item1.Charges,
                Item2 = inventoryEventDto.Item2.ItemId,
                Item2Charges = inventoryEventDto.Item2.Charges,
                Item3 = inventoryEventDto.Item3.ItemId,
                Item3Charges = inventoryEventDto.Item3.Charges,
                Item4 = inventoryEventDto.Item4.ItemId,
                Item4Charges = inventoryEventDto.Item4.Charges,
                Item5 = inventoryEventDto.Item5.ItemId,
                Item5Charges = inventoryEventDto.Item5.Charges,
                BackPack0 = inventoryEventDto.BackPack0.ItemId,
                BackPack0Charges = inventoryEventDto.BackPack0.Charges,
                BackPack1 = inventoryEventDto.BackPack1.ItemId,
                BackPack1Charges = inventoryEventDto.BackPack1.Charges,
                BackPack2 = inventoryEventDto.BackPack2.ItemId,
                BackPack2Charges = inventoryEventDto.BackPack2.Charges,
                Teleport0 = inventoryEventDto.Teleport0.ItemId,
                Teleport0Charges = inventoryEventDto.Teleport0.Charges,
                Neutral0 = inventoryEventDto.Neutral0.ItemId,
                Neutral0Charges = inventoryEventDto.Neutral0.Charges
            };
        }

        // TODO - el campo assist es una lista
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

        // TODO - Mapear los atributos
        public SpiritBearInventoryEvent mapSpiritBearInventoryEvent(
                            SpiritBearInventoryEventDto spiritBearInventoryEventDto, long matchPlayerId)
        {
            return new SpiritBearInventoryEvent
            {
                MatchPlayerId = matchPlayerId,
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
