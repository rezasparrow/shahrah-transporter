using Shahrah.Transporter.Domain.Models.DataTransferObjects;
using Shahrah.Transporter.Domain.Enums;
using Shahrah.Transporter.Application.Drivers.Models;

namespace Shahrah.Transporter.Application.OrderItems.Models;

public class OrderItemDto
{
    public int Id { get; set; }
    public Guid CorrelationId { get; set; }
    public int OrderId { get; set; }
    public string LoadTitle { get; set; }
    public string LoadDescrption { get; set; }
    public decimal Value { get; set; }
    public decimal AcceptedPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? PaymentDeadlineExpiredTime { get; set; }
    public AddressDto Source { get; set; }
    public AddressDto Destination { get; set; }

    public long TimeToLive => CalculateTimeToLive();

    public OrderItemStatus Status { get; set; }
    public string StatusTitle { get; set; }
    public bool IsLoadingConfirmed { get; set; }
    public bool IsTripEnded { get; set; }
    public string WaybillCode { get; set; }

    public DriverDto Driver { get; set; }
    public SenderDto Sender { get; set; }
    public string LoadReceiverFirstName { get; set; }
    public string LoadReceiverLastName { get; set; }
    public string LoadReceiverMobileNumber { get; set; }
    private long CalculateTimeToLive()
    {
        if (Status is OrderItemStatus.PendingPayment or OrderItemStatus.AttemptToPay && PaymentDeadlineExpiredTime.HasValue)
            return (long)(PaymentDeadlineExpiredTime.Value - DateTime.Now).TotalSeconds;
        return 0;
    }

    public decimal PayableAmount { get; set; }
}