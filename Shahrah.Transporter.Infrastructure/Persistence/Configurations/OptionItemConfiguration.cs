using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class OptionItemConfiguration : IEntityTypeConfiguration<OptionItem>
{
    public void Configure(EntityTypeBuilder<OptionItem> builder)
    {
        builder.Property(p => p.Value).HasColumnType("nvarchar(128)").IsRequired();

        builder.HasData(new OptionItem(1) { Value = "چادر", OptionId = 1 });
        builder.HasData(new OptionItem(2) { Value = "بدون سقف", OptionId = 1 });

        builder.HasData(new OptionItem(3) { Value = "یخچال", OptionId = 2 });
        builder.HasData(new OptionItem(4) { Value = "جرثقیل", OptionId = 2 });
        builder.HasData(new OptionItem(5) { Value = "ضربه گیر", OptionId = 2 });
    }
}