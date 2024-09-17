using MediatR;
using Parbad;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Payments.Commands.RegisterPaymentForSubscription;

public class RegisterPaymentForSubscriptionCommandHandler : IRequestHandler<RegisterPaymentForSubscriptionCommand, IPaymentRequestResult>
{
    private readonly IPaymentService _paymentService;

    public RegisterPaymentForSubscriptionCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<IPaymentRequestResult> Handle(RegisterPaymentForSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.RegisterPaymentForSubscription(request.PersonId, request.PlanId,
            request.SelectedGateway, cancellationToken);
    }
}