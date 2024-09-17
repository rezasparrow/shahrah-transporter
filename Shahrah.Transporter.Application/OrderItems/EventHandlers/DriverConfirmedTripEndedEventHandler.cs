using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

/// <summary>
/// راننده تائید میکنه که سفر تموم شده
/// </summary>
public class DriverConfirmedTripEndedEventHandler : KafkaEventHandler<DriverConfirmedTripEndedEvent>
{
    private readonly IOrderItemService _orderItemService;

    public DriverConfirmedTripEndedEventHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public override async Task Handle(DriverConfirmedTripEndedEvent message)
    {
        await _orderItemService.DriverConfirmTripEnded(message);
    }
}