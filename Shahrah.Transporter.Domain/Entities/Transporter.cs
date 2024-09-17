using Shahrah.Framework.Models;
using Shahrah.Transporter.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Shahrah.Transporter.Domain.Entities;

public class Transporter : Entity<long>
{
    public string Name { get; set; }
    public string NationalId { get; set; }
    public string LicenseSerialNumber { get; set; }
    public DateTime LicenseExpirationDate { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public TransporterActivityZoneType ActivityZone { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; }
    public ICollection<Person> People { get; set; }
}