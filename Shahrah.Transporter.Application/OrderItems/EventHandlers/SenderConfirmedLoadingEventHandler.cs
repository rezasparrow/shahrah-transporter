using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

public class SenderConfirmedLoadingEventHandler(IOrderItemService orderItemService) : KafkaEventHandler<SenderConfirmedLoadingEvent>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public override async Task Handle(SenderConfirmedLoadingEvent message)
    {
        await _orderItemService.SenderConfirmedLoading(message);
    }
}