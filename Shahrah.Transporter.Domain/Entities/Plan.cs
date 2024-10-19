using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class Plan(int id) : Entity<int>(id)
{
    public string Title { get; set; }
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}