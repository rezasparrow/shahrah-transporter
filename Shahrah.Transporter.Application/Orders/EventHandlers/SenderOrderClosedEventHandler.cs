using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.EventHandlers;

/// <summary>
/// سندر سفارش و میبنده
/// </summary>
public class SenderOrderClosedEventHandler : KafkaEventHandler<SenderOrderClosedEvent>
{
    private readonly IOrderService _orderService;

    public SenderOrderClosedEventHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override async Task Handle(SenderOrderClosedEvent message)
    {
        await _orderService.OrderClosedBySender(message.OrderId);
    }
}