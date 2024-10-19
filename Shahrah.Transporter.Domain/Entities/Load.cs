using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class Load(int id) : Entity<int>(id)
{
    public string Title { get; set; }
    public ICollection<Order> Orders { get; set; }
}