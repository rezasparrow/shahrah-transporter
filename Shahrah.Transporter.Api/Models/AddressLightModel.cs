using Shahrah.Framework.Resources;

using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class AddressLightModel
{
    [Display(Name = "City", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int CityId { get; set; }

    [Display(Name = "Latitude", ResourceType = typeof(DisplayNameResource))]
    public double Latitude { get; set; }

    [Display(Name = "Longitude", ResourceType = typeof(DisplayNameResource))]
    public double Longitude { get; set; }
}