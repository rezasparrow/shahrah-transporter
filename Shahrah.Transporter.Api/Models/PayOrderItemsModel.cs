using Shahrah.Framework.Payment;

namespace Shahrah.Transporter.Api.Models;

public class PayOrderItemsModel
{
    public GatewayType SelectedGateway { get; set; }

    public List<int> OrderItemIds { get; set; }
}