using Shahrah.Framework.Resources;

using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class ResetPasswordModel
{
    [Display(Name = "Mobile", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression("^09([0-9]{9})$", ErrorMessageResourceName = "MobileFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string Mobile { get; set; }

    [Display(Name = "Password", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(100, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "PasswordConfirm", ResourceType = typeof(DisplayNameResource))]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }
}