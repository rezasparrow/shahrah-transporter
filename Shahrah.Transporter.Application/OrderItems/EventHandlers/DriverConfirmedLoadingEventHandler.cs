using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

/// <summary>
/// راننده تائید میکنه که بارگیری انجام شده
/// </summary>
public class DriverConfirmedLoadingEventHandler : KafkaEventHandler<DriverConfirmedLoadingEvent>
{
    private readonly IOrderItemService _orderItemService;

    public DriverConfirmedLoadingEventHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public override async Task Handle(DriverConfirmedLoadingEvent message)
    {
        await _orderItemService.DriverConfirmedLoading(message);
    }
}