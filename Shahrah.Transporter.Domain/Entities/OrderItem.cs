using Shahrah.Framework.Models;
using OrderItemStatus = Shahrah.Transporter.Domain.Enums.OrderItemStatus;

namespace Shahrah.Transporter.Domain.Entities;

public class OrderItem : Entity<int>
{
    public OrderItem()
    { }

    public OrderItem(int id) : base(id)
    {
    }

    public long DriverId { get; set; }
    public int BidId { get; set; }
    public decimal OfferedPriced { get; set; }
    public decimal? PaidAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public OrderItemStatus Status { get; set; }
    public bool IsPendItem { get; set; }
    public int? PaymentId { get; set; }
    public Payment Payment { get; set; }
    public string DriverNationalCode { get; set; }
    public string VehicleSmartCardNumber { get; set; }
    public bool IsLoadingConfirmed { get; set; }
    public DateTime LoadingConfirmationDateTime { get; set; }
    public bool IsLoadingConfirmedBySender { get; set; }
    public bool IsLoadingConfirmedByDriver { get; set; }
    public string WaybillCode { get; set; }
    public bool IsTripEnded { get; set; }
    public bool IsTripEndedBySender { get; set; }
    public bool IsTripEndedByDriver { get; set; }
    public DateTime EndTripDateTime { get; set; }
    public DateTime? PaymentDeadlineExpiredTime { get; set; }
    public Guid? PayTransactionReferenceId { get; set; }
}