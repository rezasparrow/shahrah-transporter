using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.EventHandlers;

/// <summary>
/// سندر سفارش و میبنده
/// </summary>
public class SenderOrderClosedEventHandler(IOrderService orderService) : KafkaEventHandler<SenderOrderClosedEvent>
{
    private readonly IOrderService _orderService = orderService;

    public override async Task Handle(SenderOrderClosedEvent message)
    {
        await _orderService.OrderClosedBySender(message.OrderId);
    }
}