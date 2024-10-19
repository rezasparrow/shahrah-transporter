namespace Shahrah.Transporter.Application.Orders.Models;

public class RegisterOrderDto
{
    public RegisterOrderAddressDto SourceAddress { get; set; }

    public RegisterOrderAddressDto DestinationAddress { get; set; }

    public int LoadId { get; set; }

    public string LoadDescription { get; set; }

    public DateTime LoadingDate { get; set; }

    public int PackageId { get; set; }

    public string PackingTypeDescription { get; set; }

    public float Weight { get; set; }

    public decimal Value { get; set; }

    public string Description { get; set; }

    public int TruckId { get; set; }

    public IEnumerable<int> VehicleOptionItems { get; set; }

    public bool IsWeighStationRequire { get; set; }

    public decimal MinimumOfferPrice { get; set; }

    public decimal MaximumOfferPrice { get; set; }

    public int VehicleQuantity { get; set; }

    public bool IsSpecialOffer { get; set; }

    public Guid? CorrelationId { get; set; }
}

public class RegisterOrderAddressDto
{
    public int CityId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}