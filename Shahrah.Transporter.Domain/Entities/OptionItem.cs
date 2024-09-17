using Shahrah.Framework.Models;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class OptionItem : Entity<int>
{
    public OptionItem(int id) : base(id)
    {
    }

    public string Value { get; set; }
    public int OptionId { get; set; }
    public Option Option { get; set; }
    public ICollection<VehicleOptionItem> VehicleOptionItems { get; set; }
    public ICollection<OrderOptionItem> OrderOptionItems { get; set; }
}