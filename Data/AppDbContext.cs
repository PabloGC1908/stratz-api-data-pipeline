using Microsoft.EntityFrameworkCore;
using StratzAPI.Models;

namespace StratzAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Match> Match { get; set; }
        public DbSet<MatchStats> MatchStatistics { get; set; }
        public DbSet<MatchPlayer> MatchPlayer { get; set; }
        public DbSet<MatchPickBans> MatchStatsPickBans { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<League> League { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<MatchPlayerItems> MatchPlayerItem { get; set; }
        public DbSet<LeagueTeamPlayer> LeagueTeamPlayer { get; set; }
        public DbSet<Serie> Serie { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
    }
}
