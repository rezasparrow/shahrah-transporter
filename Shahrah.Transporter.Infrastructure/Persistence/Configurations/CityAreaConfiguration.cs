using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Shahrah.Framework.Data;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class CityAreaConfiguration : IEntityTypeConfiguration<CityArea>
{
    public void Configure(EntityTypeBuilder<CityArea> builder)
    {
        builder.Property(p => p.Area).HasColumnType("geometry");
        builder.HasOne(x => x.City).WithMany(x => x.Areas).HasForeignKey(x => x.CityId);
        builder.HasData(GetBaseDdata());
    }

    public static IEnumerable<CityArea> GetBaseDdata()
    {
        var baseDataCities = BaseGeometryData.GetCityAreas();
        var CityAreas = new List<CityArea>();

        foreach (var item in baseDataCities)
        {
            var areaPlogon = CreateAreaPolygon(item.Area);
            CityArea CityArea = new CityArea(item.Id)
            {
                CityId = item.CityId,
                CorrelationId = item.CorrelationId,
                Area = areaPlogon
            };
            CityAreas.Add(CityArea);
        }

        return CityAreas;
    }

    private static Polygon CreateAreaPolygon(string areaStringValue)
    {
        WKBReader s = new WKBReader();
        var data = areaStringValue.Substring(2, areaStringValue.Length - 2);
        var ba = WKBReader.HexToBytes(data);
        Geometry wkbGeom = s.Read(ba);
        wkbGeom.SRID = 4326;
        return (Polygon)wkbGeom;
    }
}

public class RawAreaData
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public Guid CorrelationId { get; set; }
    public string Area { get; set; }
}