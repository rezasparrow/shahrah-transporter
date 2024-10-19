using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Framework.Data;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(p => p.Name).HasColumnType("nvarchar(64)").IsRequired();

            builder.HasOne(p => p.Province).WithMany(p => p.Cities).HasForeignKey(p => p.ProvinceId);
            builder.HasData(GetBaseData());
        }

        public static IEnumerable<City> GetBaseData()
        {
            var baseDataCities = BaseGeometryData.GetCities();
            var cities = baseDataCities.Select(t => new City(t.Id)
            {
                Name = t.Name,
                CorrelationId = t.CorrelationId,
                ProvinceId = t.ProvinceId
            });
            return cities;
        }
    }
}