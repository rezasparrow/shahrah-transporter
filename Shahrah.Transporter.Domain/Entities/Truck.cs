using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Truck : Entity<int>
{
    public Truck(int id) : base(id)
    {
    }

    public string Title { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
    public double MinLoadWeight { get; set; }
    public double? MaxLoadWeight { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }
    public ICollection<Order> Orders { get; set; }
}