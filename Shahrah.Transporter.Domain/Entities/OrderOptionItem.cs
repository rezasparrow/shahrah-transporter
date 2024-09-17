namespace Shahrah.Transporter.Domain.Entities;

public class OrderOptionItem
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int OptionItemId { get; set; }
    public OptionItem OptionItem { get; set; }
}