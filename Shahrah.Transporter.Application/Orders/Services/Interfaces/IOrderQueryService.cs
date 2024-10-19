using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Orders.Services.Interfaces;

public interface IOrderQueryService
{
    Task<Order> GetOrderWithAllDataAsync(long? personId, int orderId);
}