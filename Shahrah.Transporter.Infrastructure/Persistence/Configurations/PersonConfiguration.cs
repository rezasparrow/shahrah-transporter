using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedOnAdd().Metadata.ValueGenerated = ValueGenerated.Never;
        builder.Property(p => p.TransporterId).IsRequired();
        builder.Property(p => p.FirstName).HasColumnType("nvarchar(64)").IsRequired();
        builder.Property(p => p.LastName).HasColumnType("nvarchar(128)").IsRequired();
        builder.Property(p => p.NationalCode).HasColumnType("varchar(10)").IsRequired();
        builder.Property(p => p.BirthDate);
        builder.Property(p => p.MobileNumber).HasColumnType("varchar(11)").IsRequired();
        builder.Property(x => x.Status).IsRequired().HasDefaultValue(PersonStatus.Active);
        builder.Property(x => x.AgentRegistrationStatus).IsRequired().HasDefaultValue(AgentRegistrationStatus.None);
        builder.Property(p => p.LicenseSerialNumber);

        builder.HasIndex(p => p.NationalCode).HasFilter("[IsDeleted]=(0) AND [AgentRegistrationStatus]<>(20)").IsUnique();
        builder.HasIndex(p => p.MobileNumber).HasFilter("[IsDeleted]=(0) AND [AgentRegistrationStatus]<>(20)").IsUnique();

        builder.HasMany(p => p.Payments).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);
        builder.HasMany(cp => cp.Subscriptions).WithOne(c => c.Person).HasForeignKey(cp => cp.PersonId);
        builder.HasMany(cp => cp.FinancialTransactions).WithOne(c => c.Person).HasForeignKey(cp => cp.PersonId);
    }
}