using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Services;

internal class OrderQueryService : IOrderQueryService
{
    private readonly IApplicationDbContext _dbContext;

    public OrderQueryService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> GetOrderWithAllDataAsync(long? personId, int orderId)
    {
        return await _dbContext.Orders
            .Include(q => q.Source).ThenInclude(q => q.City).ThenInclude(q => q.Province)
            .Include(q => q.Destination).ThenInclude(q => q.City).ThenInclude(q => q.Province)
            .Include(q => q.Person).ThenInclude(q => q.Transporter)
            .Include(q => q.OrderOptionItems).ThenInclude(q => q.OptionItem).ThenInclude(q => q.Option)
            .Include(x => x.Load)
            .Include(x => x.Package)
            .Include(x => x.Receivers).ThenInclude(r => r.Person)
            .Include(x => x.OrderItems)
            .SingleOrDefaultAsync(q => q.Id == orderId
                                       && (q.PersonId == personId || q.Receivers.Any(x => x.PersonId == personId)));
    }
}