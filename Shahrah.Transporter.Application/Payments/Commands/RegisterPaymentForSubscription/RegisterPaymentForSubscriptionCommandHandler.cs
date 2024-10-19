using MediatR;
using Parbad;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;

namespace Shahrah.Transporter.Application.Payments.Commands.RegisterPaymentForSubscription;

public class RegisterPaymentForSubscriptionCommandHandler(IPaymentService paymentService) : IRequestHandler<RegisterPaymentForSubscriptionCommand, IPaymentRequestResult>
{
    private readonly IPaymentService _paymentService = paymentService;

    public async Task<IPaymentRequestResult> Handle(RegisterPaymentForSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.RegisterPaymentForSubscription(request.PersonId, request.PlanId,
            request.SelectedGateway, cancellationToken);
    }
}