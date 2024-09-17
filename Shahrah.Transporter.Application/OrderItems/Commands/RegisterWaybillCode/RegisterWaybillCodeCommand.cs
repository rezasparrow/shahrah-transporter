using MediatR;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.OrderItems.Commands.RegisterWaybillCode;

/// <summary>
/// ثبت کد 17 رقمی بارنامه
/// </summary>
public class RegisterWaybillCodeCommand : IRequest, ITransactionalCommand
{
    public int OrderItemId { get; }
    public long PersonId { get; }
    public string WaybillCode { get; }

    public RegisterWaybillCodeCommand(int orderItemId, string waybillCode, long personId)
    {
        OrderItemId = orderItemId;
        WaybillCode = waybillCode;
        PersonId = personId;
    }
}