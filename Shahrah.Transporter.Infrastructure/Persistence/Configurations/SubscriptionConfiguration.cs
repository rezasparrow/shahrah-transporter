using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Subscription> builder)
    {
        builder.HasOne(p => p.Plan).WithMany(x => x.Subscriptions).HasForeignKey(x => x.PlanId);
        builder.HasOne(p => p.Person).WithMany(x => x.Subscriptions).HasForeignKey(x => x.PersonId);
        builder.HasOne(p => p.Payment).WithOne(x => x.Subscription).HasForeignKey<Payment>(x => x.SubscriptionId);
    }
}