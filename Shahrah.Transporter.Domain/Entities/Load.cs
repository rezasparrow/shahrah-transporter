using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Load : Entity<int>
{
    public Load(int id) : base(id)
    {
    }

    public string Title { get; set; }
    public ICollection<Order> Orders { get; set; }
}