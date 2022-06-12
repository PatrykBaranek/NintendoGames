using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesList.Entities
{
    public class NintendoDbContext : DbContext
    {
        public NintendoDbContext(DbContextOptions<NintendoDbContext> options) : base(options)
        {

        }

        public DbSet<GamesEntity> Games { get; set; }
        public DbSet<PricesEntity> Prices { get; set; }
        public DbSet<RatingsEntity> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamesEntity>()
                .Property(g => g.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<GamesEntity>()
                .HasOne(g => g.Prices)
                .WithOne(p => p.Games)
                .HasForeignKey<PricesEntity>(p => p.GameId);

            modelBuilder.Entity<GamesEntity>()
                .HasOne(g => g.Ratings)
                .WithOne(r => r.Games)
                .HasForeignKey<RatingsEntity>(r => r.GameId);
        }
    }
}
