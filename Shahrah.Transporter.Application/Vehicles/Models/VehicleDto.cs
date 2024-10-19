using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Vehicles.Models;

public class VehicleDto
{
    public int Id { get; set; }
    public PlateNumberDto PlateNumber { get; set; }
    public string TruckTypeTitle { get; set; }
    public float NetLoadingCapacity { get; set; }
    public float GrossLoadingCapacity { get; set; }
    public List<string> VehicleOptionItemTitles { get; set; }
    public DriverAssigningStatus AssigningStatus { get; set; }
    public long? DriverId { get; set; }
    public string DriverFirstName { get; set; }
    public string DriverLastName { get; set; }
    public string DriverNationalCode { get; set; }
}

public enum DriverAssigningStatus
{
    Unassigned = 0,
    Assinged = 1
}