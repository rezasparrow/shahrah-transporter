using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class Province(int id) : Entity<int>(id)
{
    public string Name { get; set; }
    public ICollection<City> Cities { get; set; }
}