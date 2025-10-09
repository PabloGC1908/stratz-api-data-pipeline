using StratzAPI.Models.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace StratzAPI.Models
{
    public class MatchPlayer
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public Match? Match { get; set; }
        public long PlayerId { get; set; }
        public Player? Player { get; set; }
        public bool? IsRadiant { get; set; }
        public short? HeroId { get; set; }
        public byte? Kills { get; set; }
        public byte? Deaths { get; set; }
        public byte? Assists { get; set; }
        public short? NumLastHits { get; set; }
        public short? NumDenies { get; set; }
        public short? GoldPerMinute { get; set; }
        public int? Networth { get; set; }
        public short? ExperiencePerMinute { get; set; }
        public short? Level { get; set; }
        public int? Gold { get; set; }
        public int? GoldSpent { get; set; }
        public int? HeroDamage { get; set; }
        public int? TowerDamage { get; set; }
        public int? HeroHealing { get; set; }
        public string? Lane { get; set; }
        public string? Position { get; set; }
        public string? Role { get; set; }
        public string? RoleBasic { get; set; }
        public string? Award { get; set; }

        public MatchPlayerItems? MatchPlayerItems { get; set; }
        public ICollection<AbilityLearnEvent> AbilityLearnEvents { get; set; }
        public ICollection<AbilityUsedEvent> AbilityUsedEvents { get; set; }
        public ICollection<AssistEvent> AssistEvents { get; set; }
        public ICollection<BuyBackEvent>? BuyBackEvents { get;set; }
        public ICollection<CsEvent> CsEvents { get; set; }
        public ICollection<ExperienceEvent> ExperienceEvents { get; set; }
        public ICollection<GoldEvent>? GoldEvents { get; set; }
        public ICollection<HealEvent> HealEvents { get; set; }
        public ICollection<HeroDamageEvent> HeroDamageEvents { get; set; }
        public ICollection<InventoryEvent> InventoryEvents { get; set; }
        public ICollection<KillEvent> KillEvents { get; set; }
        public ICollection<PlayerUpdateAttributeEvent> PlayerUpdateAttributesEvents { get; set; }
        public ICollection<PlayerUpdateBattleEvent> PlayerUpdateBattleEvents { get; set; }
        public ICollection<PlayerUpdateGoldEvent> PlayerUpdateGoldEvents { get; set;}
        public ICollection<PlayerUpdateHealthEvent> PlayerUpdateHealthEvents { get; set; }
        public ICollection<PlayerUpdateLevelEvent> PlayerUpdateLevelEvents { get; set; }
        public ICollection<PlayerUpdatePositionEvent> PlayerUpdatePositionEvents { get; set; }
        public ICollection<PurchaseEvent> PurchaseEvents { get; set; }
        public ICollection<RuneEvent> RunesEvents { get; set; }
        public ICollection<StreakEvent> StreakEvents { get; set; }
        public ICollection<TowerDamageEvent> TowerDamageEvents { get; set; }
        public ICollection<SpiritBearInventoryEvent>? SpiritBearInventoryEvents { get; set; }
    }
}
