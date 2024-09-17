using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.EventHandlers;

public class SenderConfirmedLoadingEventHandler : KafkaEventHandler<SenderConfirmedLoadingEvent>
{
    private readonly IOrderItemService _orderItemService;

    public SenderConfirmedLoadingEventHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public override async Task Handle(SenderConfirmedLoadingEvent message)
    {
        await _orderItemService.SenderConfirmedLoading(message);
    }
}