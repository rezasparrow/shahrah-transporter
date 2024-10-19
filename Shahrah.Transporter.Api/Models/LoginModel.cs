using Shahrah.Framework.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class LoginModel
{
    [Display(Name = "Mobile", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression("^09([0-9]{9})$", ErrorMessageResourceName = "MobileFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string MobileNumber { get; set; }

    [Display(Name = "OtpCorrelationId", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public Guid OtpCorrelationId { get; set; }
}