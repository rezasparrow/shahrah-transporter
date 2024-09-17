using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.People.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class PersonModel
{
    [Display(Name = "FirstName", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(64, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string FirstName { get; set; }

    [Display(Name = "LastName", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(128, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string LastName { get; set; }

    [Display(Name = "NationalCode", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [RegularExpression(@"\d{10}", ErrorMessageResourceName = "NationalCodeLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    [StringLength(10, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string NationalCode { get; set; }

    [Display(Name = "BirthDate", ResourceType = typeof(DisplayNameResource))]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public DateTime BirthDate { get; set; }

    public RegisterPersonDto ToRegisterPersonDto(long personId, string mobileNumber)
    {
        return new RegisterPersonDto
        {
            Id = personId,
            FirstName = FirstName,
            LastName = LastName,
            NationalCode = NationalCode,
            BirthDate = BirthDate.Date,
            MobileNumber = mobileNumber
        };
    }
}