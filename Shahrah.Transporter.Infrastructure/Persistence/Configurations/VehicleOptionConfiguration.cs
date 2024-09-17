using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class VehicleOptionConfiguration : IEntityTypeConfiguration<VehicleOptionItem>
{
    public void Configure(EntityTypeBuilder<VehicleOptionItem> builder)
    {
        builder.HasIndex(cp => new { cp.VehicleId, cp.OptionItemId }).HasFilter("[IsDeleted]=(0)").IsUnique();
        builder.HasOne(cp => cp.Vehicle).WithMany(c => c.VehicleOptionItems).HasForeignKey(cp => cp.VehicleId);
        builder.HasOne(cp => cp.OptionItem).WithMany(p => p.VehicleOptionItems).HasForeignKey(cp => cp.OptionItemId);
    }
}