using Microsoft.EntityFrameworkCore;

namespace NintendoGames.Entities
{
    public class NintendoDbContext : DbContext
    {
        public NintendoDbContext(DbContextOptions<NintendoDbContext> options) : base(options)
        {

        }

        public DbSet<Game> Game { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Developers> Developers { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<GameWishList> GameWishList { get; set; }
        public DbSet<Role> Role { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(builder =>
            {
                builder.HasData(new List<Role>
                {
                    new()
                    {
                        Id = 1,
                        Name = "User"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Admin"
                    }
                });

            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.Property(u => u.Email)
                    .IsRequired();

                builder.HasOne(u => u.WishList)
                    .WithOne(w => w.User)
                    .HasForeignKey<WishList>(w => w.UserId);
            });

            modelBuilder.Entity<WishList>(builder =>
            {
                builder.HasOne(w => w.User)
                    .WithOne(u => u.WishList)
                    .HasForeignKey<User>(w => w.WishListId);
            });

            modelBuilder.Entity<Game>(builder =>
            {
                builder.Property(g => g.Title)
                    .IsRequired();

                builder.Property(g => g.ReleaseDate)
                    .IsRequired();

                builder.HasOne(g => g.Rating)
                    .WithOne(r => r.Game)
                    .HasForeignKey<Rating>(r => r.GameId);

                builder.HasMany(g => g.Developers)
                    .WithOne(d => d.Game)
                    .HasForeignKey(d => d.GameId);

                builder.HasMany(g => g.Genres)
                    .WithOne(ge => ge.Game)
                    .HasForeignKey(ge => ge.GameId);
            });

            modelBuilder.Entity<GameWishList>(builder =>
            {
                builder.HasKey(gw => new { gw.GameId, gw.WishListId });

                builder.HasOne(gw => gw.Game)
                    .WithMany(b => b.GameWishLists)
                    .HasForeignKey(gw => gw.GameId);

                builder.HasOne(gw => gw.WishList)
                    .WithMany(w => w.GameWishLists)
                    .HasForeignKey(gw => gw.WishListId);

            });

            modelBuilder.Entity<Rating>(builder =>
            {
                builder.HasOne(r => r.Game)
                    .WithOne(g => g.Rating)
                    .HasForeignKey<Game>(g => g.RatingId);
            });

            modelBuilder.Entity<Genres>(builder =>
            {
                builder.HasOne(ge => ge.Game)
                    .WithMany(g => g.Genres)
                    .HasForeignKey(ge => ge.GameId);
            });

            modelBuilder.Entity<Developers>(builder =>
            {
                builder.HasOne(d => d.Game)
                    .WithMany(g => g.Developers)
                    .HasForeignKey(d => d.GameId);
            });
        }
    }
}
