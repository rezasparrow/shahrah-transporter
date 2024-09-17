using Shahrah.Framework.Payment;
using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class PaySubscriptionModel
{
    [Display(Name = "Gateway")]
    public GatewayType SelectedGateway { get; set; } = GatewayType.ParbadVirtual;

    public int PlanId { get; set; }
}