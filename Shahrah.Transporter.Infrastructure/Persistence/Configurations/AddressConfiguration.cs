using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.Latitude).HasColumnType("float").IsRequired();
            builder.Property(x => x.Longitude).HasColumnType("float").IsRequired();

            builder.HasOne(p => p.City).WithMany(x => x.Addresses).HasForeignKey(x => x.CityId).IsRequired();
        }
    }
}