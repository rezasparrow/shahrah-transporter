using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class VehicleOptionItem : Entity<int>
{
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int OptionItemId { get; set; }
    public OptionItem OptionItem { get; set; }
}