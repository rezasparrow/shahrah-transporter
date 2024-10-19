using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.Transporters.Models;
using Shahrah.Transporter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class RegisterTransporterModel
{
    [Display(Name = "Name", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(128, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string Name { get; set; }

    [Display(Name = "NationalId", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression(@"\d{11}", ErrorMessageResourceName = "NationalIdLenghtError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(11, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string NationalId { get; set; }

    [Display(Name = "LicenseSerialNumber", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string LicenseSerialNumber { get; set; }

    [Display(Name = "LicenseExpirationDate", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public DateTime LicenseExpirationDate { get; set; }

    [Display(Name = "City", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int CityId { get; set; }

    [Display(Name = "Address", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(1024, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string Address { get; set; }

    [Display(Name = "PostalCode", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression(@"\b(?!(\d)\1{3})[13-9]{4}[1346-9][013-9]{5}\b", ErrorMessageResourceName = "PostalCodeFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(10, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PostalCode { get; set; }

    [Display(Name = "Phone", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression("^0([0-9]{10})$", ErrorMessageResourceName = "PhoneFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(11, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PhoneNumber { get; set; }

    [Display(Name = "ActivityZone", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public TransporterActivityZoneType ActivityZone { get; set; }

    [Display(Name = "Latitude", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public double Latitude { get; set; }

    [Display(Name = "Longitude", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public double Longitude { get; set; }

    public PersonModel Manager { get; set; }

    public RegisterTransporterDto ToRegisterTransporterDto()
    {
        return new RegisterTransporterDto
        {
            Name = Name,
            NationalId = NationalId,
            LicenseSerialNumber = LicenseSerialNumber,
            LicenseExpirationDate = LicenseExpirationDate,
            CityId = CityId,
            Address = Address,
            PostalCode = PostalCode,
            PhoneNumber = PhoneNumber,
            TransporterActivityZone = ActivityZone,
            Latitude = Latitude,
            Longitude = Longitude
        };
    }
}