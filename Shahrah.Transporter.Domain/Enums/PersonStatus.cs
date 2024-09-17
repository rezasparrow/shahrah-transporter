using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Domain.Enums;

public enum PersonStatus
{
    [Display(Name = "غیر فعال")]
    DeActive = 0,

    [Display(Name = "فعال")]
    Active = 1,

    [Display(Name = "معلق")]
    Suspended = 2
}