using Shahrah.Transporter.Application.Vehicles.Models;
using System;

namespace Shahrah.Transporter.Application.Drivers.Models
{
    public class DriverDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseExpirationDate { get; set; }
        public VehicleDto Vehicle { get; set; }
    }
}
