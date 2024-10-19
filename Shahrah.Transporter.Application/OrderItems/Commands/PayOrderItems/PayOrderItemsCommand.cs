using MediatR;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItems;

/// <summary>
/// پرداخت لاین آیتمها
/// </summary>
public class PayOrderItemsCommand(GatewayType selectedGateway, IList<int> orderItemsId) : IRequest<IPaymentRequestResult>, ITransactionalCommand
{
    public IList<int> OrderItemsId { get; set; } = orderItemsId;
    public GatewayType SelectedGateway { get; set; } = selectedGateway;
}