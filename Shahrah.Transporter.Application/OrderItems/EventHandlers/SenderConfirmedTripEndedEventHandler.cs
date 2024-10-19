using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

public class SenderConfirmedTripEndedEventHandler(IOrderItemService orderItemService) : KafkaEventHandler<SenderConfirmedTripEndedEvent>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public override async Task Handle(SenderConfirmedTripEndedEvent message)
    {
        await _orderItemService.SenderConfirmedTripEnded(message);
    }
}