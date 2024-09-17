using Shahrah.Framework.Payment;
using System.Collections.Generic;

namespace Shahrah.Transporter.Api.Models;

public class PayOrderItemsModel
{
    public GatewayType SelectedGateway { get; set; }

    public List<int> OrderItemIds { get; set; }
}