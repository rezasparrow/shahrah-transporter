using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(p => p.Title).HasColumnType("nvarchar(128)").IsRequired();
        builder.Property(p => p.Days).IsRequired();
        builder.Property(p => p.Price).IsRequired();

        builder.HasData(new Plan(1) { Title = "یک ماهه", Days = 30, Price = 1000000 ,Description = "یک ماه" });
        builder.HasData(new Plan(2) { Title = "سه ماهه", Days = 90, Price = 2800000 , Description = "سه ماهه"});
        builder.HasData(new Plan(3) { Title = "شش ماهه", Days = 180, Price = 5500000 , Description  = "شش ماهه" });

        builder.HasMany(cp => cp.Subscriptions).WithOne(c => c.Plan).HasForeignKey(cp => cp.PlanId);
    }
}