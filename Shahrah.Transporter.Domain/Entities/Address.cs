using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Address : Entity<int>
{
    public int CityId { get; set; }
    public City City { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<Order> SourceOrders { get; set; }
    public ICollection<Order> DestinationOrders { get; set; }
}