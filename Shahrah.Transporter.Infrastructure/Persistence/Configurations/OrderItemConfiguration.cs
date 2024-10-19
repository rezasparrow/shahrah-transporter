using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(p => p.DriverId).IsRequired();
            builder.Property(p => p.OfferedPriced).IsRequired();
            builder.Property(p => p.BidId).IsRequired();
            builder.Property(p => p.DriverNationalCode).HasColumnType("varchar(10)");
            builder.Property(p => p.VehicleSmartCardNumber).HasColumnType("nvarchar(25)");
            builder.Property(p => p.WaybillCode).HasColumnType("varchar(17)");
            builder
                .Property(p => p.CorrelationId)
                .HasDefaultValue(Guid.NewGuid());


            builder.HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
        }
    }
}