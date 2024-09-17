using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(p => p.OwnerFirstName).HasColumnType("nvarchar(64)");
        builder.Property(p => p.OwnerLastName).HasColumnType("nvarchar(128)");
        builder.Property(p => p.OwnerNationalCode).HasColumnType("varchar(10)");
        builder.Property(p => p.PlateNumber).HasColumnType("nvarchar(10)").IsRequired();
        builder.Property(p => p.Vin).HasColumnType("varchar(17)").IsRequired();
        builder.Property(p => p.SmartCardNumber).HasColumnType("nvarchar(25)").IsRequired();
        builder.Property(p => p.SmartCardExpirationDate).IsRequired();
        builder.Property(p => p.NetLoadingCapacity).IsRequired();
        builder.Property(p => p.GrossLoadingCapacity).IsRequired();
        builder.Property(p => p.IsTransporterVehicleOwner).IsRequired();

        builder.Property(p => p.DriverId);
        builder.Property(p => p.DriverFirstName).HasColumnType("nvarchar(64)");
        builder.Property(p => p.DriverLastName).HasColumnType("nvarchar(128)");
        builder.Property(p => p.DriverNationalCode).HasColumnType("char(10)");

        builder.HasOne(p => p.Truck).WithMany(x => x.Vehicles).HasForeignKey(x => x.TruckId);
        builder.HasOne(p => p.Transporter).WithMany(x => x.Vehicles).HasForeignKey(x => x.TransporterId);
    }
}