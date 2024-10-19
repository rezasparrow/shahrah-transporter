using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Domain.Entities;

public class Vehicle : Entity<int>
{
    public Vehicle()
    { }

    public Vehicle(int id) : base(id)
    {
    }

    public string? OwnerFirstName { get; set; }
    public string? OwnerLastName { get; set; }
    public string? OwnerNationalCode { get; set; }
    public required string PlateNumber { get; set; }
    public required string Vin { get; set; }
    public required string SmartCardNumber { get; set; }
    public DateTime SmartCardExpirationDate { get; set; }
    public float NetLoadingCapacity { get; set; }
    public float GrossLoadingCapacity { get; set; }
    public bool IsTransporterVehicleOwner { get; set; }
    public long? DriverId { get; set; }
    public string? DriverFirstName { get; set; }
    public string? DriverLastName { get; set; }
    public string? DriverNationalCode { get; set; }
    public long TransporterId { get; set; }
    public Transporter Transporter { get; set; }
    public int TruckId { get; set; }
    public Truck Truck { get; set; }
    public ICollection<VehicleOptionItem> VehicleOptionItems { get; set; }
}