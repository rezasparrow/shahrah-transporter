using MediatR;
using Shahrah.Transporter.Application.Orders.Models;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Queries.GetOrder;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IOrderQueryService _orderQueryService;

    public GetOrderQueryHandler(IOrderQueryService orderQueryService)
    {
        _orderQueryService = orderQueryService;
    }

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var item = await _orderQueryService.GetOrderWithAllDataAsync(request.PersonId, request.OrderId);
        return new OrderDto(item);
    }
}