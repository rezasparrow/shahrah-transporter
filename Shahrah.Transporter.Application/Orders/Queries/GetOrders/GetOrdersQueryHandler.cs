using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Models;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _dbContext.Orders
            .Include(q => q.Source).ThenInclude(q => q.City).ThenInclude(q => q.Province)
            .Include(q => q.Destination).ThenInclude(q => q.City).ThenInclude(q => q.Province)
            .Include(q => q.Truck)
            .Include(q => q.Receivers)
            .Include(q => q.OrderItems)
            .Include(q => q.OrderOptionItems)
            .ThenInclude(q => q.OptionItem)
            .Where(q => (q.PersonId == request.PersonId ||
                         !q.PersonId.HasValue && q.Receivers.Any(x => x.PersonId == request.PersonId))
                        && !(q.Status == OrderStatus.Closed && q.ModifiedDate.HasValue &&
                             q.ModifiedDate < DateTime.Now.AddHours(-24))
            )
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);

        return orders.Select(item => new OrderDto(item));
    }
}