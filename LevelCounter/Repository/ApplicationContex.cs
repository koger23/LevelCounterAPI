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
    }
}
