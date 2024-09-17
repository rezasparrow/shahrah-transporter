using System.Collections.Generic;

namespace Shahrah.Transporter.Application.Vehicles.Models;

public class ReadyVehiclesLookupDto
{
    public int Id { get; set; }
    public string DriverFirstName { get; set; }
    public string DriverLastName { get; set; }
    public string TruckTypeTitle { get; set; }
    public List<string> VehicleOptionItemTitles { get; set; }
}