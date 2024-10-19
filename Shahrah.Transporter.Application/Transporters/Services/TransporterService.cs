using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Transporters.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.Transporters.Services;

public class TransporterService(IApplicationDbContext dbContext) : ITransporterService
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Person>> GetActivePersonsByTransportersLatLong(double x, double y)
    {
        var area = await GetCityArea(x, y);
        if (area == null) return Enumerable.Empty<Person>();

        var provinceId = area.City?.ProvinceId;
        var people = await _dbContext.People
            .Include(person => person.Transporter)
            .Where(person => person.Status == PersonStatus.Active
                             && (person.Transporter.CityId == area.CityId &&
                                 person.Transporter.ActivityZone == TransporterActivityZoneType.City
                                 || person.Transporter.City.ProvinceId == provinceId &&
                                 person.Transporter.ActivityZone == TransporterActivityZoneType.Province
                                 || person.Transporter.ActivityZone == TransporterActivityZoneType.Country))
            .ToListAsync();
        return people;
    }

    private async Task<CityArea> GetCityArea(double x, double y)
    {
        var point = new Point(x, y)
        {
            SRID = 4326
        };
        var area = await _dbContext.Areas
            .Include(cityArea => cityArea.City)
            .ThenInclude(city => city.Province)
            .Where(p => p.Area.Contains(point))
            .SingleOrDefaultAsync();
        return area;
    }
}