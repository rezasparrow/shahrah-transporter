using Shahrah.Framework.Scheduling;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.OrderItems.Jobs;

/// <summary>
/// مهلت پرداخت از زمانی که به درگاه بانک رفته اید تمام شد
/// همون تایم 15 دقیقه
/// </summary>
public class OrderItemAttemptToPayExpiredJob : DelayedJob
{
    private readonly IOrderItemService _orderItemService;
    private readonly IApplicationDbContext _dbContext;

    public OrderItemAttemptToPayExpiredJob(IOrderItemService orderItemService, IApplicationDbContext dbContext)
    {
        _orderItemService = orderItemService;
        _dbContext = dbContext;
    }

    public override async Task RunAsync(Dictionary<string, string> data)
    {
        // TODO: Javad Rasouli >> برای جابها قابلیت بازکردن و بستن خودکار ترنزکشن فراهم شود
        var orderItemId = GetData<int>("OrderItemId");
        await using var tran = await _dbContext.BeginTransactionAsync();
        try
        {
            await _orderItemService.PendingPaymentExpired(orderItemId);
            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
        }
    }
}