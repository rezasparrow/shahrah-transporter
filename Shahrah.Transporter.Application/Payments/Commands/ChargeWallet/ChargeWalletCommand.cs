using MediatR;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.ChargeWallet;

/// <summary>
/// شارژ کیف پول
/// </summary>
public class ChargeWalletCommand : IRequest<IPaymentRequestResult>, ITransactionalCommand
{
    public GatewayType SelectedGateway { get; set; }
    public decimal Amount { get; set; }
    public long PersonId { get; set; }

    public ChargeWalletCommand(GatewayType selectedGateway, decimal amount, long personId)
    {
        Amount = amount;
        SelectedGateway = selectedGateway;
        PersonId = personId;
    }
}