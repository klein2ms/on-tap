using Microsoft.EntityFrameworkCore;
using OnTap.Blazor.Server.Models;

namespace OnTap.Blazor.Server.Data
{
    public class DraftContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueSettings> LeagueSettings { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Draft> Drafts { get; set; }
        public DbSet<DraftPick> DraftPicks { get; set; }

        public DraftContext(DbContextOptions<DraftContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            builder.Entity<LeagueSettings>()
                .HasAlternateKey(s => new { s.LeagueId });

            builder.Entity<DraftPosition>()                
                .HasAlternateKey(p => new { p.DraftId, p.TeamId });

            builder.Entity<DraftPick>()
                .HasAlternateKey(p => new { p.DraftId, p.TeamId, p.PlayerId });

        }
    }
}