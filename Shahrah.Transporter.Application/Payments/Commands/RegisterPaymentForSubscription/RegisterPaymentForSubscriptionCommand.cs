using MediatR;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.RegisterPaymentForSubscription;

/// <summary>
/// خرید اشتراک
/// </summary>
public class RegisterPaymentForSubscriptionCommand : IRequest<IPaymentRequestResult>, ITransactionalCommand
{
    public RegisterPaymentForSubscriptionCommand(GatewayType selectedGateway, int planId, long personId)
    {
        SelectedGateway = selectedGateway;
        PlanId = planId;
        PersonId = personId;
    }

    public long PersonId { get; set; }

    public GatewayType SelectedGateway { get; }

    public int PlanId { get; }
}