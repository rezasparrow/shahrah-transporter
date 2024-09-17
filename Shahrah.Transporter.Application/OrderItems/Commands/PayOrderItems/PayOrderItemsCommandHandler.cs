using Parbad;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItems;

public class PayOrderItemsCommandHandler : MediatR.IRequestHandler<PayOrderItemsCommand, IPaymentRequestResult>
{
    private readonly IOrderItemPaymentService _orderItemPaymentService;

    public PayOrderItemsCommandHandler(IOrderItemPaymentService orderItemPaymentService)
    {
        _orderItemPaymentService = orderItemPaymentService;
    }

    public async Task<IPaymentRequestResult> Handle(PayOrderItemsCommand request, CancellationToken cancellationToken)
    {
        return await _orderItemPaymentService.Pay(request.OrderItemsId, request.SelectedGateway, cancellationToken);
    }
}