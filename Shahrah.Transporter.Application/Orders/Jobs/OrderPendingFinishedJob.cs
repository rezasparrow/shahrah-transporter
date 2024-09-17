using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Scheduling;
using Shahrah.Transporter.Application.Common.Interfaces;
using SlimMessageBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Jobs;

public class OrderPendingFinishedJob : DelayedJob
{
    private readonly IMessageBus _messageBus;
    private readonly IApplicationDbContext _dbContext;

    public OrderPendingFinishedJob(IMessageBus messageBus, IApplicationDbContext dbContext)
    {
        _messageBus = messageBus;
        _dbContext = dbContext;
    }

    public override async Task RunAsync(Dictionary<string, string> data)
    {
        var orderId = GetData<int>("OrderId");
        var order = await _dbContext.Orders.SingleAsync(order => order.Id == orderId);
        await _messageBus.Publish(new OrderPendingFinishedEvent(order.CorrelationId, order.Id, order.SenderRequestId));
    }
}