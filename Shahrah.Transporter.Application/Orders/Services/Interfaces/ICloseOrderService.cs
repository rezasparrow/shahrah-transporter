using Shahrah.Transporter.Application.Orders.Commands.CloseOrder;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Services.Interfaces;

public interface ICloseOrderService
{
    Task Close(CloseOrderTypeEnum closeType, int orderId, long? personId,
        CancellationToken cancellationToken = default);
}