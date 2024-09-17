using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Models;

namespace Shahrah.Transporter.Application.Orders.Commands.RegisterOrder;

/// <summary>
/// ثبت سفارش توسط تی سی
/// </summary>
public class RegisterOrderCommand : IRequest<Unit>, ITransactionalCommand
{
    public RegisterOrderCommand(RegisterOrderDto order, long personId)
    {
        Order = order;
        PersonId = personId;
    }

    public RegisterOrderDto Order { get; }
    public long PersonId { get; }
}