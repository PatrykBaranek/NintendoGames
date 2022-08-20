using Microsoft.EntityFrameworkCore;

namespace NintendoGames.Entities
{
    public class NintendoDbContext : DbContext
    {
        public NintendoDbContext(DbContextOptions<NintendoDbContext> options) : base(options)
        {

        }

        public DbSet<Games> Games { get; set; }
        public DbSet<Ratings> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Games>()
                .Property(g => g.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Games>()
                .HasOne(g => g.Ratings)
                .WithOne(r => r.Games)
                .HasForeignKey<Ratings>(r => r.GameId);
        }
    }
}
