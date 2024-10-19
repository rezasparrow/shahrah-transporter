using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class Truck(int id) : Entity<int>(id)
{
    public string Title { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
    public double MinLoadWeight { get; set; }
    public double? MaxLoadWeight { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }
    public ICollection<Order> Orders { get; set; }
}