using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

public class SenderConfirmedTripEndedEventHandler : KafkaEventHandler<SenderConfirmedTripEndedEvent>
{
    private readonly IOrderItemService _orderItemService;

    public SenderConfirmedTripEndedEventHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public override async Task Handle(SenderConfirmedTripEndedEvent message)
    {
        await _orderItemService.SenderConfirmedTripEnded(message);
    }
}