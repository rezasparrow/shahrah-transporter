using Shahrah.Framework.Scheduling;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.Jobs;

/// <summary>
/// مدت زمان لازم برای قیمت گذاری توسط تی سی برای سفارش تمام میشه
/// </summary>
public class OrderPricingFinishedJob : DelayedJob
{
    private readonly IOrderPricingService _orderPricingService;
    private readonly IApplicationDbContext _dbContext;

    public OrderPricingFinishedJob(IOrderPricingService orderPricingService, IApplicationDbContext dbContext)
    {
        _orderPricingService = orderPricingService;
        _dbContext = dbContext;
    }

    public override async Task RunAsync(Dictionary<string, string> data)
    {
        // TODO: Javad Rasouli >> برای جابها قابلیت بازکردن و بستن خودکار ترنزکشن فراهم شود
        var orderId = GetData<int>("OrderId");
        await using var tran = await _dbContext.BeginTransactionAsync();
        try
        {
            await _orderPricingService.OrderPricingFinished(orderId);
            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
        }
    }
}