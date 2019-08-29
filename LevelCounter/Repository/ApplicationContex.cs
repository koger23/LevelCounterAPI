using LevelCounter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LevelCounter.Repository
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Statistics> Statistics { get; set; }
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<InGameUser> InGameUsers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(au => au.Relationships)
                .WithOne(r => r.User);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(au => au.Games)
                .WithOne(g => g.ApplicationUser);
            modelBuilder.Entity<Game>()
                .HasMany(au => au.InGameUsers)
                .WithOne(g => g.Game);
        }
    }
}
