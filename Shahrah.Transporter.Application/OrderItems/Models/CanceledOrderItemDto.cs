namespace Shahrah.Transporter.Application.OrderItems.Models;

public class CanceledOrderItemDto : OrderItemDto
{
    public CanceledOrderItemDto()
    {
    }

    public long TrackingNumber { get; set; }
}