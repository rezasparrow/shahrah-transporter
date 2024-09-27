using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(p => p.LoadDescription).HasColumnType("nvarchar(1024)");
        builder.Property(p => p.PackingTypeDescription).HasColumnType("nvarchar(64)");
        builder.Property(p => p.Weight);
        builder.Property(p => p.Value).IsRequired();
        builder.Property(p => p.Description).HasColumnType("nvarchar(1024)");
        builder.Property(p => p.IsWeighStationRequire).IsRequired();
        builder.Property(p => p.MinimumOfferPrice).IsRequired();
        builder.Property(p => p.MaximumOfferPrice).IsRequired();
        builder.Property(p => p.TransporterOfferPrice);
        builder.Property(p => p.SenderOfferPrice);
        builder.Property(p => p.IsSpecialOffer).IsRequired();
        builder.Property(p => p.VehicleQuantity).IsRequired();
        builder.Property(p => p.SendingDate).IsRequired();
        builder.Property(p => p.LoadingDate).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.SenderRequestId);
        builder.Property(p => p.SenderName);
        builder
            .Property(p => p.SenderMobileNumber)
            .IsRequired(false);

        builder.HasOne(p => p.Package).WithMany(x => x.Orders).HasForeignKey(x => x.PackageId);
        builder.HasOne(p => p.Load).WithMany(x => x.Orders).HasForeignKey(x => x.LoadId);
        builder.HasOne(p => p.Truck).WithMany(x => x.Orders).HasForeignKey(x => x.TruckId);
        builder.HasOne(x => x.Source).WithMany(x => x.SourceOrders).HasForeignKey(x => x.SourceId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Destination).WithMany(x => x.DestinationOrders).HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(p => p.Person).WithMany(x => x.Orders).HasForeignKey(x => x.PersonId);
    }
}