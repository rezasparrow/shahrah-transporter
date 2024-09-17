using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Package : Entity<int>
{
    public Package(int id) : base(id)
    {
    }

    public string Title { get; set; }

    public ICollection<Order> Orders { get; set; }
}