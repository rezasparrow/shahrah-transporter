using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Models;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrderSenderInfo;

public class GetOrderSenderInfoQueryHandler(IApplicationDbContext dbContext, IPersonService subscriptionService) : IRequestHandler<GetOrderSenderInfoQuery, OrderSenderInfoDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IPersonService _subscriptionService = subscriptionService;

    public async Task<OrderSenderInfoDto> Handle(GetOrderSenderInfoQuery request, CancellationToken cancellationToken)
    {
        if (!await _subscriptionService.HasSubscription(request.PersonId)) return new OrderSenderInfoDto();

        var item = await _dbContext.Orders.SingleAsync(order => (order.PersonId == request.PersonId || !order.PersonId.HasValue) && order.Id == request.OrderId, cancellationToken);
        return new OrderSenderInfoDto
        {
            Name = item.SenderName,
            MobileNumber = item.SenderMobileNumber
        };
    }
}