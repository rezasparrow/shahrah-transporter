using Microsoft.EntityFrameworkCore;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class TransporterConfiguration : IEntityTypeConfiguration<Domain.Entities.Transporter>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entities.Transporter> builder)
    {
        builder.Property(p => p.Name).HasColumnType("nvarchar(128)").IsRequired();
        builder.Property(p => p.NationalId).HasColumnType("varchar(11)").IsRequired();
        builder.Property(p => p.LicenseSerialNumber).HasColumnType("nvarchar(64)").IsRequired();
        builder.Property(x => x.Address).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(p => p.PostalCode).HasColumnType("varchar(10)").IsRequired();
        builder.Property(p => p.PhoneNumber).HasColumnType("varchar(11)").IsRequired();
        builder.Property(x => x.ActivityZone).HasColumnType("tinyint").IsRequired();
        builder.Property(x => x.Latitude).HasColumnType("float").IsRequired();
        builder.Property(x => x.Longitude).HasColumnType("float").IsRequired();
        builder.HasOne(p => p.City).WithMany(x => x.Companies).HasForeignKey(x => x.CityId);
        builder.HasMany(cp => cp.People).WithOne(c => c.Transporter).HasForeignKey(cp => cp.TransporterId);
    }
}