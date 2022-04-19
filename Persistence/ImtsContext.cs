using Domain;
using Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ImtsContext : DbContext
    {
        public ImtsContext(DbContextOptions<ImtsContext> options)
        : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().Property(c => c.id).HasColumnName("projectId");
            builder.Entity<Office>().Property(c => c.id).HasColumnName("officeId");
        }

    }
}