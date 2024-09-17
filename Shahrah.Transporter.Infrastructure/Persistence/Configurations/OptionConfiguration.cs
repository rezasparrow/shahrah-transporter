using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.Property(p => p.Title).HasColumnType("nvarchar(64)").IsRequired();
        builder.Property(p => p.Type).IsRequired();
        builder.HasMany(p => p.Items).WithOne(x => x.Option).HasForeignKey(x => x.OptionId);

        builder.HasData(new Option(1) { Title = "نوع سقف", Type = OptionType.Single });
        builder.HasData(new Option(2) { Title = "امکانات اضافی", Type = OptionType.Multiple });
    }
}