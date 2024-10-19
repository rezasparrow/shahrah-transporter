using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Domain.Entities;

public class Order : Entity<int>
{
    public float Weight { get; set; }
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public bool IsWeighStationRequire { get; set; }
    public decimal MinimumOfferPrice { get; set; }
    public decimal MaximumOfferPrice { get; set; }
    public decimal? TransporterOfferPrice { get; set; }
    public decimal? SenderOfferPrice { get; set; }
    public bool IsSpecialOffer { get; set; }
    public int VehicleQuantity { get; set; }
    public int VehicleQuantityInSearch { get; set; }
    public DateTime SendingDate { get; set; }
    public DateTime LoadingDate { get; set; }
    public OrderStatus Status { get; set; }
    public int PackageId { get; set; }
    public Package Package { get; set; }
    public string PackingTypeDescription { get; set; }
    public int LoadId { get; set; }
    public Load Load { get; set; }
    public string LoadDescription { get; set; }
    public int TruckId { get; set; }
    public Truck Truck { get; set; }
    public int SourceId { get; set; }
    public Address Source { get; set; }
    public int DestinationId { get; set; }
    public Address Destination { get; set; }
    public long? PersonId { get; set; }
    public Person Person { get; set; }
    public ICollection<OrderOptionItem> OrderOptionItems { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public int? SenderRequestId { get; set; }
    public string? SenderName { get; set; }
    public string? SenderMobileNumber { get; set; }
    public long? SenderUserId { get; set; }
    public ICollection<PersonOrder> Receivers { get; set; }
    public DateTime? SearchOrPendingOrPricingDeadlineExpiredTime { get; set; }
}