using Microsoft.EntityFrameworkCore;
using StratzAPI.Models;

namespace StratzAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Match> Match { get; set; }
        public DbSet<MatchStatistics> MatchStatistics { get; set; }
        public DbSet<MatchPlayer> MatchPlayer { get; set; }
        public DbSet<MatchStatsPickBans> MatchStatsPickBans { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<League> League { get; set; }
        public DbSet<Player> Player { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
    }
}
