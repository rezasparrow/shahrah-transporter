using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class City : Entity<int>
{
    public City(int id) : base(id)
    {
    }

    public string Name { get; set; }
    public int ProvinceId { get; set; }
    public Province Province { get; set; }
    public ICollection<Transporter> Companies { get; set; }
    public ICollection<Address> Addresses { get; set; }
    public ICollection<CityArea> Areas { get; set; }
}