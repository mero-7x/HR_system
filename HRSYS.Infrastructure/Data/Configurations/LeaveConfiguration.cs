using HRSYS.Domain.Entities;
using HRSYS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRSYS.Infrastructure.Data.Configurations
{
    public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Type)
                   .HasConversion<string>()       
                   .IsRequired();

            builder.Property(l => l.Status)
                   .HasConversion<string>()  
                   .HasDefaultValue(LeaveStatus.Pending)     
                   .IsRequired();

            builder.Property(l => l.Reason)
                   .HasMaxLength(200);

                     builder.HasOne(l => l.Employee)
                            .WithMany(e => e.Leaves)
                            .HasForeignKey(l => l.EmployeeId);

                     builder.Property(l => l.CreatedAt)
                             .HasDefaultValueSql("CURRENT_TIMESTAMP");
                   
        }
    }
}
