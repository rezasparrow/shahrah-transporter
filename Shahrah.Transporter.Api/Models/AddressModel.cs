using Shahrah.Framework.Resources;

using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class AddressModel
{
    [Display(Name = "City", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int CityId { get; set; }

    [Display(Name = "PostalCode", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(10, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression("^([0-9]{10})$", ErrorMessageResourceName = "PostalCodeFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PostalCode { get; set; }

    [Display(Name = "Description", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(256, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string Description { get; set; }

    [Display(Name = "Latitude", ResourceType = typeof(DisplayNameResource))]
    public double? Latitude { get; set; }

    [Display(Name = "Longitude", ResourceType = typeof(DisplayNameResource))]
    public double? Longitude { get; set; }
}