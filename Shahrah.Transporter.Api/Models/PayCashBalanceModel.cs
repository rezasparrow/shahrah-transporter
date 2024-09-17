using Shahrah.Framework.Payment;

namespace Shahrah.Transporter.Api.Models;

public class PayCashBalanceModel
{
    public GatewayType SelectedGateway { get; set; }

    public decimal Amount { get; set; }
}