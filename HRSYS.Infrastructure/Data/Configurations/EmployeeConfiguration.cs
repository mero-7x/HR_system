using HRSYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRSYS.Infrastructure.Data.Configurations
{
       public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
       {
              public void Configure(EntityTypeBuilder<Employee> builder)
              {
                     builder.HasKey(e => e.Id);

                     builder.Property(e => e.FullName)
                            .IsRequired()
                            .HasMaxLength(100);

                     builder.Property(e => e.BaseSalary)
                            .HasColumnType("decimal(18,2)");

                     builder.Property(e => e.CurrentDegree)
                            .HasConversion<string>()
                            .IsRequired();

              //        builder.Property(e => e.DepartmentId)
              //       .HasDefaultValueSql("Unassigned");

                     builder.Property(e => e.Gender)
                            .HasConversion<string>()
                            .IsRequired();

                     //      builder.Property(e => e.Role)
                     //             .HasConversion<string>()      
                     //             .IsRequired();

                     builder.HasOne(e => e.Department)
                            .WithMany(d => d.Employees)
                            .HasForeignKey(e => e.DepartmentId);

                     builder.Property(u => u.CreatedAt)
                     .HasDefaultValueSql("CURRENT_TIMESTAMP");
              }
       }
}
