using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Plan : Entity<int>
{
    public Plan(int id) : base(id)
    {
    }

    public string Title { get; set; }
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}