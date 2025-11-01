using HRSYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRSYS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Leave> Leaves => Set<Leave>();
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType.IsEnum ||
                                (Nullable.GetUnderlyingType(p.PropertyType)?.IsEnum ?? false));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<string>();
                }
            }

            // modelBuilder.Entity<Employee>()
            // .HasOne(e => e.User)
            //  .WithOne(u => u.Employee)
            //  .HasForeignKey<User>(u => u.EmployeeId)
            //  .OnDelete(DeleteBehavior.SetNull);

            // modelBuilder.Entity<User>()
            // .HasOne(u => u.Employee)
            // .WithOne(e => e.User)
            // .HasForeignKey<User>(u => u.EmployeeId)
            // .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                 .HasOne(d => d.Manager)
             .WithOne(u => u.ManagedDepartment)
                        .HasForeignKey<Department>(d => d.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);


         


            base.OnModelCreating(modelBuilder);
        }
    }
}
