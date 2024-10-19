using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

/// <summary>
/// راننده سفارشی که قبلا قبول کرده رو لغو میکنه
/// </summary>
public class BidCanceledEventHandler(IOrderItemService orderItemService) : KafkaEventHandler<BidCanceledEvent>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public override async Task Handle(BidCanceledEvent message)
    {
        await _orderItemService.BidCanceledByDriver(message);
    }
}