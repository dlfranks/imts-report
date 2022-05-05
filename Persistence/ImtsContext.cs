using Domain.imts;
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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UsersInOfficeRole> UsersInOfficeRoles { get; set; }
        public DbSet<UserOfficeRole> UserOfficeRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().Property(c => c.id).HasColumnName("projectId");
            builder.Entity<Office>().Property(c => c.id).HasColumnName("officeId");
            builder.Entity<UsersInOfficeRole>().HasKey(q => new { q.officeId, q.userRoleId, q.employeeId });
            builder.Entity<UsersInOfficeRole>().HasOne(o => o.office).WithMany().HasForeignKey(o => o.officeId);
            builder.Entity<UsersInOfficeRole>().HasOne(o => o.employee).WithMany().HasForeignKey(o => o.employeeId);
            builder.Entity<UsersInOfficeRole>().HasOne(o => o.userRole).WithMany().HasForeignKey(o => o.userRoleId);

            builder.Entity<Employee>().Property(e => e.id).HasColumnName("employeeId");
        }

    }
}