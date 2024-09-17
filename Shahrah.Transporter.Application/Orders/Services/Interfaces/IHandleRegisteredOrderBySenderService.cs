using Shahrah.Framework.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Services.Interfaces;

public interface IHandleRegisteredOrderBySenderService
{
    Task Handle(OrderRegisteredBySenderEvent orderRegisteredBySenderEvent, CancellationToken cancellationToken = default);
}