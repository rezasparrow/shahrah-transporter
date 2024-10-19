using Shahrah.Framework.Events;

namespace Shahrah.Transporter.Application.Orders.Services.Interfaces;

public interface IHandleRegisteredOrderBySenderService
{
    Task Handle(OrderRegisteredBySenderEvent orderRegisteredBySenderEvent, CancellationToken cancellationToken = default);
}