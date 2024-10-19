using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class Package(int id) : Entity<int>(id)
{
    public string Title { get; set; }

    public ICollection<Order> Orders { get; set; }
}