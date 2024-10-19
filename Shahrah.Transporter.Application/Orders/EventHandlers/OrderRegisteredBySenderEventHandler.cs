using Shahrah.Framework.Events;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.EventHandlers;

/// <summary>
/// سفارش توسط سندر ثبت شده
/// </summary>
public class OrderRegisteredBySenderEventHandler(IHandleRegisteredOrderBySenderService handleRegisteredOrderBySenderService) : KafkaEventHandler<OrderRegisteredBySenderEvent>
{
    private readonly IHandleRegisteredOrderBySenderService _handleRegisteredOrderBySenderService = handleRegisteredOrderBySenderService;

    public override async Task Handle(OrderRegisteredBySenderEvent orderRegisteredBySenderEvent)
    {
        await _handleRegisteredOrderBySenderService.Handle(orderRegisteredBySenderEvent);
    }
}