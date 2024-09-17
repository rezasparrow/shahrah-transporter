using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class OrderOptionConfiguration : IEntityTypeConfiguration<OrderOptionItem>
{
    public void Configure(EntityTypeBuilder<OrderOptionItem> builder)
    {
        builder.HasKey(cp => new { cp.OrderId, cp.OptionItemId });

        builder.HasOne(cp => cp.Order).WithMany(c => c.OrderOptionItems).HasForeignKey(cp => cp.OrderId);
        builder.HasOne(cp => cp.OptionItem).WithMany(p => p.OrderOptionItems).HasForeignKey(cp => cp.OptionItemId);
    }
}