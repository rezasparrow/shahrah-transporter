using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.TrackingNumber).IsRequired();
        builder.Property(p => p.Amount).IsRequired();
        builder.Property(p => p.IsPaid).IsRequired();
        builder.Property(p => p.TransactionCode).HasColumnType("varchar(64)");
        builder.Property(p => p.GatewayName).HasColumnType("nvarchar(64)").IsRequired();
        builder.Property(p => p.GatewayAccountName).HasColumnType("nvarchar(64)").IsRequired();
        builder.Property(p => p.Message).HasColumnType("nvarchar(max)");

        builder.HasMany(x => x.OrderItems).WithOne(x => x.Payment).HasForeignKey(x => x.PaymentId);
        builder.HasOne(p => p.Person).WithMany(x => x.Payments).HasForeignKey(x => x.PersonId);
    }
}