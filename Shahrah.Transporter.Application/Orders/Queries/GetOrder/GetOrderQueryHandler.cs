using MediatR;
using Shahrah.Transporter.Application.Orders.Models;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrder;

public class GetOrderQueryHandler(IOrderQueryService orderQueryService) : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IOrderQueryService _orderQueryService = orderQueryService;

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var item = await _orderQueryService.GetOrderWithAllDataAsync(request.PersonId, request.OrderId);
        return new OrderDto(item);
    }
}