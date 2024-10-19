using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class OptionItem(int id) : Entity<int>(id)
{
    public string Value { get; set; }
    public int OptionId { get; set; }
    public Option Option { get; set; }
    public ICollection<VehicleOptionItem> VehicleOptionItems { get; set; }
    public ICollection<OrderOptionItem> OrderOptionItems { get; set; }
}