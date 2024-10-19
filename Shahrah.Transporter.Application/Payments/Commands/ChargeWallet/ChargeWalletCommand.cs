using MediatR;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.ChargeWallet;

/// <summary>
/// شارژ کیف پول
/// </summary>
public class ChargeWalletCommand(GatewayType selectedGateway, decimal amount, long personId) : IRequest<IPaymentRequestResult>, ITransactionalCommand
{
    public GatewayType SelectedGateway { get; set; } = selectedGateway;
    public decimal Amount { get; set; } = amount;
    public long PersonId { get; set; } = personId;
}