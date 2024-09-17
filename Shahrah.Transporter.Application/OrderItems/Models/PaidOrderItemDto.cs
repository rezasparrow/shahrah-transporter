using System;

namespace Shahrah.Transporter.Application.OrderItems.Models;

public class PaidOrderItemDto : OrderItemDto
{
    public long TrackingNumber { get; set; }
    public string VehicleSmartCardNumber { get; set; }

    public decimal? PaidAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
}