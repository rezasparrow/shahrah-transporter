using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Shahrah.Transporter.Application.Orders.Commands.HideOrder;

internal class HideOrderCommandHandler : IRequestHandler<HideOrderCommand, Unit>
{
    public HideOrderCommandHandler()
    {
    }

    public async Task<Unit> Handle(HideOrderCommand request, CancellationToken cancellationToken)
    {
        //todo: commented by Hadi, it needs to be implemented in other way

        // var order = await _unitOfWork.PersonOrders.FindAsync(x => x.OrderId == request.OrderId && x.TransporterId == request.TransporterId);

        // order.IsHide = true;

        // await _unitOfWork.PersonOrders.UpdateAsync(order);
        // await _dbContext.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}