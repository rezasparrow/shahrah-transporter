using Shahrah.Transporter.Domain.Models.DataTransferObjects;
using Shahrah.Transporter.Domain.Enums;
using Shahrah.Transporter.Application.Drivers.Models;

namespace Shahrah.Transporter.Application.OrderItems.Models;

public class TripEndedOrderItemDto
{
    public int Id { get; set; }
    public Guid CorrelationId { get; set; }
    public int OrderId { get; set; }
    public string LoadTitle { get; set; }
    public string LoadDescription { get; set; }
    public decimal Value { get; set; }
    public decimal AcceptedPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public AddressDto Source { get; set; }
    public AddressDto Destination { get; set; }
    public OrderItemStatus Status { get; set; }
    public string StatusTitle { get; set; }
    public bool IsLoadingConfirmed { get; set; }
    public bool IsTripEnded { get; set; }
    public string WaybillCode { get; set; }
    public decimal? PaidAmount { get; set; }
    public DateTime? PayemntDate { get; set; }
    public DateTime EndTripDateTime { get; set; }
    public SenderDto Sender { get; set; }
    public DriverDto Driver { get; set; }
}