using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Vehicles.Models;

public class AddVehicleDto
{
    public PlateNumberDto PlateNumber { get; set; }

    public string Vin { get; set; }

    public string SmartCardNumber { get; set; }

    public DateTime SmartCardExpirationDate { get; set; }

    public int TruckTypeId { get; set; }

    public float NetLoadingCapacity { get; set; }

    public float GrossLoadingCapacity { get; set; }

    public List<int> VehicleOptionItems { get; set; }

    public bool IsTransporterVehicleOwner { get; set; }

    public string OwnerFirstName { get; set; }

    public string OwnerLastName { get; set; }

    public string OwnerNationalCode { get; set; }
}