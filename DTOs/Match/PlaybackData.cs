using StratzAPI.DTOs.Match.Events;

namespace StratzAPI.DTOs.Match
{
    public class PlaybackData
    {
        public List<AbilityLearnEventDto>? AbilityLearnEvents { get; set; }
        public List<AbilityUsedEventDto>? AbilityUsedEvents { get; set; }
        public List<ItemUsedEventDto>? ItemUsedEvents { get; set; }
        public List<PlayerUpdatePositionEventDto>? PlayerUpdatePositionEvents { get; set; }
        public List<PlayerUpdateGoldEventDto>? PlayerUpdateGoldEvents { get; set; }
        public List<PlayerUpdateAttributeEventDto>? PlayerUpdateAttributeEvents { get; set; }
        public List<PlayerUpdateLevelEventDto>? PlayerUpdateLevelEvents { get; set; }
        public List<PlayerUpdateHealthEventDto>? PlayerUpdateHealthEvents { get; set; }
        public List<PlayerUpdateBattleEventDto>? PlayerUpdateBattleEvents { get; set; }
        public List<KillEventDto>? KillEvents { get; set; }
        public List<AssistEventDto>? AssistEvents { get; set; }
        public List<CsEventDto>? CsEvents { get; set; }
        public List<GoldEventDto>? GoldEvents { get; set; }
        public List<ExperienceEventDto>? ExperienceEvents { get; set; }
        public List<HealEventDto>? HealEvents { get; set; }
        public List<HeroDamageEventDto>? HeroDamageEvents { get; set; }
        public List<TowerDamageEventDto>? TowerDamageEvents { get; set; }
        public List<InventoryEventDto>? InventoryEvents { get; set; }
        public List<PurchaseEventDto>? PurchaseEvents { get; set; }
        public List<BuyBackEventDto>? BuyBackEvents { get; set; }
        public List<StreakEventDto>? StreakEvents { get; set; }
        public List<RuneEventDto>? RuneEvents { get; set; }
        public List<SpiritBearInventoryEventDto>? SpiritBearInventoryEvents { get; set; }
    }
}
