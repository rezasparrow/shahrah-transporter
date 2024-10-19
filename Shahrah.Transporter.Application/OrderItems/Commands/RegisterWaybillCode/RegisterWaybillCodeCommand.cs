using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.RegisterWaybillCode;

/// <summary>
/// ثبت کد 17 رقمی بارنامه
/// </summary>
public class RegisterWaybillCodeCommand(int orderItemId, string waybillCode, long personId) : IRequest, ITransactionalCommand
{
    public int OrderItemId { get; } = orderItemId;
    public long PersonId { get; } = personId;
    public string WaybillCode { get; } = waybillCode;
}