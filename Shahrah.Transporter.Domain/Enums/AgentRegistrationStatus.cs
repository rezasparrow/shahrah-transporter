using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Domain.Enums;

public enum AgentRegistrationStatus
{
    [Display(Name = "ثبت شده")]
    None = 0,

    [Display(Name = "در انتظار تایید")]
    Pending = 10,

    [Display(Name = "رد شده")]
    Revoked = 20,

    [Display(Name = "ثبت شده")]
    Registered = 30
}