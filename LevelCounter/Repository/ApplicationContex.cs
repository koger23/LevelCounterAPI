using LevelCounter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LevelCounter.Repository
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Statistics> Statistics { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRelationships>()
                .HasKey(ur => new { ur.ApplicationUserId, ur.RelationshipId });

            modelBuilder.Entity<UserRelationships>()
                .HasOne(ur => ur.Relationship)
                .WithMany(r => r.Relationships)
                .HasForeignKey(ur => ur.RelationshipId);

            modelBuilder.Entity<UserRelationships>()
                .HasOne(ur => ur.ApplicationUser)
                .WithMany(au => au.Relationships)
                .HasForeignKey(ur => ur.ApplicationUserId);
        }
    }
}
