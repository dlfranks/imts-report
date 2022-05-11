using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppContext : IdentityDbContext<AppUser>
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }
        public DbSet<OfficeRole> OfficeRoles { get; set; }
        public DbSet<AppUserOfficeRole> AppUserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OfficeRole>().HasIndex(q => q.RoleName).IsClustered(false);
            builder.Entity<AppUserOfficeRole>().HasKey(q => new { q.AppuserId, q.ImtsOfficeId });

        }
    }
}