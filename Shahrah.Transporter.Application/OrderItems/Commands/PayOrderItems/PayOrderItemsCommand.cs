using MediatR;
using Parbad;
using Shahrah.Framework.Payment;
using Shahrah.Transporter.Application.Common.Interfaces;
using System.Collections.Generic;

namespace Shahrah.Transporter.Application.OrderItems.Commands.PayOrderItems;

/// <summary>
/// پرداخت لاین آیتمها
/// </summary>
public class PayOrderItemsCommand : IRequest<IPaymentRequestResult>, ITransactionalCommand
{
    public IList<int> OrderItemsId { get; set; }
    public GatewayType SelectedGateway { get; set; }

    public PayOrderItemsCommand(GatewayType selectedGateway, IList<int> orderItemsId)
    {
        OrderItemsId = orderItemsId;
        SelectedGateway = selectedGateway;
    }
}