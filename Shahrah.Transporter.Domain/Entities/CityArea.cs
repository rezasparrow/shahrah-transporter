using NetTopologySuite.Geometries;
using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class CityArea : Entity<int>
{
    public CityArea(int id) : base(id)
    {
    }

    public int CityId { get; set; }
    public City City { get; set; }
    public Polygon Area { get; set; }
}