using MediatR;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.RegisterPaymentForSubscription;

/// <summary>
/// خرید اشتراک
/// </summary>
public class RegisterPaymentForSubscriptionCommand(GatewayType selectedGateway, int planId, long personId) : IRequest<IPaymentRequestResult>, ITransactionalCommand
{
    public long PersonId { get; set; } = personId;

    public GatewayType SelectedGateway { get; } = selectedGateway;

    public int PlanId { get; } = planId;
}