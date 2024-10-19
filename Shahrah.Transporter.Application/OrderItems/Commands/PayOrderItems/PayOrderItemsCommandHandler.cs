using Parbad;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItems;

public class PayOrderItemsCommandHandler(IOrderItemPaymentService orderItemPaymentService) : MediatR.IRequestHandler<PayOrderItemsCommand, IPaymentRequestResult>
{
    private readonly IOrderItemPaymentService _orderItemPaymentService = orderItemPaymentService;

    public async Task<IPaymentRequestResult> Handle(PayOrderItemsCommand request, CancellationToken cancellationToken)
    {
        return await _orderItemPaymentService.Pay(request.OrderItemsId, request.SelectedGateway, cancellationToken);
    }
}