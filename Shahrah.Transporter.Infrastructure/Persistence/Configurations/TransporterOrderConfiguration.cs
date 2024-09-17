using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations
{
    public class TransporterOrderConfiguration : IEntityTypeConfiguration<PersonOrder>
    {
        public void Configure(EntityTypeBuilder<PersonOrder> builder)
        {
            builder.Property(x => x.OfferedPrice);
            builder.HasOne(x => x.Order).WithMany(x => x.Receivers).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.Person).WithMany(x => x.ReceivedOrders).HasForeignKey(x => x.PersonId);
        }
    }
}