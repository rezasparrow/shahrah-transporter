using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Province : Entity<int>
{
    public Province(int id) : base(id)
    {
    }

    public string Name { get; set; }
    public ICollection<City> Cities { get; set; }
}