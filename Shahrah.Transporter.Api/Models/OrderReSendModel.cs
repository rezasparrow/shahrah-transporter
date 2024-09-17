using Shahrah.Framework.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class OrderReSendModel
{
    [Display(Name = "MinimumOfferPrice", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public long MinimumOfferPrice { get; set; }

    [Display(Name = "MaximumOfferPrice", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public long MaximumOfferPrice { get; set; }

    [Display(Name = "VehicleQuantity", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public int VehicleQuantity { get; set; }
}