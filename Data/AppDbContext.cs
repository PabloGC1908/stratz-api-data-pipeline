using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StratzAPI.Models;
using StratzAPI.Models.Events;
using StratzAPI.Models.MatchEvents;

namespace StratzAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Match> Match { get; set; }
        public DbSet<MatchStats> MatchStats { get; set; }
        public DbSet<MatchPlayer> MatchPlayer { get; set; }
        public DbSet<MatchPickBans> MatchPickBans { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<League> League { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<MatchPlayerItems> MatchPlayerItems { get; set; }
        public DbSet<LeagueTeamPlayer> LeagueTeamPlayer { get; set; }
        public DbSet<Serie> Serie { get; set; }

        public DbSet<AbilityLearnEvent> AbilityLearnEvent { get; set; }
        public DbSet<AbilityUsedEvent> AbilityUsedEvent { get; set; }
        public DbSet<AssistEvent> AssistEvent { get; set; }
        public DbSet<BuyBackEvent> BuyBackEvent { get; set; }
        public DbSet<CsEvent> CsEvent { get; set; }
        public DbSet<ExperienceEvent> ExperienceEvent { get; set; }
        public DbSet<GoldEvent> GoldEvent { get; set; }
        public DbSet<HealEvent> HealEvent { get; set; }
        public DbSet<HeroDamageEvent> HeroDamageEvent { get; set; }
        public DbSet<InventoryEvent> InventoryEvent { get; set; }
        public DbSet<KillEvent> KillEvent { get; set; }
        public DbSet<PlayerUpdateAttributeEvent> PlayerUpdateAttributeEvent { get; set; }
        public DbSet<PlayerUpdateBattleEvent> PlayerUpdateBattleEvent { get; set; }
        public DbSet<PlayerUpdateGoldEvent> PlayerUpdateGoldEvent { get; set; }
        public DbSet<PlayerUpdateHealthEvent> PlayerUpdateHealthEvent { get; set; }
        public DbSet<PlayerUpdateLevelEvent> PlayerUpdateLevelEvent { get; set; }
        public DbSet<PlayerUpdatePositionEvent> PlayerUpdatePositionEvent { get; set; }
        public DbSet<PurchaseEvent> PurchaseEvent { get; set; }
        public DbSet<RuneEvent> RuneEvent { get; set; }
        public DbSet<SpiritBearInventoryEvent> SpiritBearInventoryEvent { get; set; }
        public DbSet<StreakEvent> StreakEvent { get; set; }
        public DbSet<TowerDamageEvent> TowerDamageEvent { get; set; }
        public DbSet<BuildingEvent> BuildingEvent { get; set; }
        public DbSet<MatchRuneEvent> MatchRuneEvent { get; set; }
        public DbSet<RoshanEvent> RoshanEvent { get; set; }
        public DbSet<TowerDeathEvent> TowerDeathEvent { get; set; }
        public DbSet<WardEvent> WardEvent { get; set; }
        public DbSet<CourierEvent> CourierEvent { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
    }
}
