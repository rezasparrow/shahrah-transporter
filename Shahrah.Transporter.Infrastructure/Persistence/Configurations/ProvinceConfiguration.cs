using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Framework.Data;
using Shahrah.Transporter.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.Property(p => p.Name).HasColumnType("nvarchar(64)").IsRequired();
        builder.HasData(GetBaseData());
    }

    public static IEnumerable<Province> GetBaseData()
    {
        var baseDataProvinces = BaseGeometryData.GetProvinces();
        var peovinces = baseDataProvinces.Select(t => new Province(t.Id)
        {
            Name = t.Name,
            CorrelationId = t.CorrelationId,
        });
        return peovinces;
    }
}