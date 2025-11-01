using HRSYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRSYS.Infrastructure.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(d => d.Name).IsUnique();

            builder.Property(u => u.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(d => d.ManagerEmployee)
           .WithMany()
           .HasForeignKey(d => d.ManagerEmployeeId)
           .OnDelete(DeleteBehavior.SetNull);


            builder.Property(u => u.IsActive)
           .HasDefaultValue(true);
        }
    }
}
