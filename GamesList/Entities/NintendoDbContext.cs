using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;

namespace GamesList.Entities
{
    public class NintendoDbContext : DbContext
    {
        public NintendoDbContext(DbContextOptions<NintendoDbContext> options) : base(options)
        {

        }

        public DbSet<GamesEntity> Games { get; set; }
        public DbSet<RatingsEntity> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamesEntity>()
                .Property(g => g.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<GamesEntity>()
                .HasOne(g => g.Ratings)
                .WithOne(r => r.Games)
                .HasForeignKey<RatingsEntity>(r => r.GameId);
        }
    }
}
