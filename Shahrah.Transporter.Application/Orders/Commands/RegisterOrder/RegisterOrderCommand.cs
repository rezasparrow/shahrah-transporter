using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Commands.RegisterOrder;

/// <summary>
/// ثبت سفارش توسط تی سی
/// </summary>
public class RegisterOrderCommand(RegisterOrderDto order, long personId) : IRequest<Unit>, ITransactionalCommand
{
    public RegisterOrderDto Order { get; } = order;
    public long PersonId { get; } = personId;
}