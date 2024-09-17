namespace Shahrah.Transporter.Api.Models;

public class FindDriverModel
{
    public decimal MinimumOfferPrice { get; set; }
    public decimal MaximumOfferPrice { get; set; }
    public int VehicleQuantity { get; set; }
}