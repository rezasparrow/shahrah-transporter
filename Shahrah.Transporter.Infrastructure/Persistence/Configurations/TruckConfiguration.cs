using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Framework.Data;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class TruckConfiguration : IEntityTypeConfiguration<Truck>
{
    public void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder.Property(p => p.Title).HasColumnType("nvarchar(128)").IsRequired();
        builder.Property(p => p.Length).IsRequired();
        builder.Property(p => p.Width).IsRequired();
        builder.Property(p => p.Height).IsRequired();
        builder.Property(p => p.MinLoadWeight).IsRequired();
        builder.Property(p => p.MaxLoadWeight);

        builder.HasData(BaseTruckData.GetTrucks().Select(t => new Truck(t.Id)
        {
            Title = t.Title,
            Length = t.Length,
            Height = t.Height,
            Width = t.Width,
            MinLoadWeight = t.MinLoadWeight,
            MaxLoadWeight = t.MaxLoadWeight
        }));
    }
}