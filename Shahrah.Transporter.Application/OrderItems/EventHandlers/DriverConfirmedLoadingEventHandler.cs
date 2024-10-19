using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

/// <summary>
/// راننده تائید میکنه که بارگیری انجام شده
/// </summary>
public class DriverConfirmedLoadingEventHandler(IOrderItemService orderItemService) : KafkaEventHandler<DriverConfirmedLoadingEvent>
{
    private readonly IOrderItemService _orderItemService = orderItemService;

    public override async Task Handle(DriverConfirmedLoadingEvent message)
    {
        await _orderItemService.DriverConfirmedLoading(message);
    }
}