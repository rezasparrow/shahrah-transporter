using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.OrderItems.Models;
using Shahrah.Transporter.Domain.Enums;
using Shahrah.Transporter.Domain.Models.DataTransferObjects;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetPendingPaymentOrderItems;

public class GetPendingPaymentOrderItemsQueryHandler(IApplicationDbContext dbContext,
     AppSettings appSettings) : IRequestHandler<GetPendingPaymentOrderItemsQuery, IEnumerable<OrderItemDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly AppSettings _appSettings = appSettings;

    public async Task<IEnumerable<OrderItemDto>> Handle(GetPendingPaymentOrderItemsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.OrderItems
            .Include(x => x.Order).ThenInclude(x => x.Source).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Destination).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Load)
            .Where(q => q.Order.PersonId == request.PersonId && q.Status == OrderItemStatus.PendingPayment || q.Status == OrderItemStatus.AttemptToPay)
            .Select(item => new OrderItemDto
            {
                Id = item.Id,
                Value = item.Order.Value,
                LoadTitle = item.Order.Load.Title,
                LoadDescrption = item.Order.LoadDescription,
                AcceptedPrice = item.OfferedPriced,
                OrderId = item.OrderId,
                CreatedDate = item.CreatedDate,
                Status = item.Status,
                StatusTitle = item.Status.GetDisplayName(),
                IsLoadingConfirmed = item.IsLoadingConfirmed,
                IsTripEnded = item.IsTripEnded,
                PayableAmount = _appSettings.ShahrahCommissionFromFindingDriver * item.OfferedPriced,
                Destination = new AddressDto
                {
                    CityId = item.Order.Destination.CityId,
                    CityName = item.Order.Destination.City.Name,
                    ProvinceId = item.Order.Destination.City.ProvinceId,
                    ProvinceName = item.Order.Destination.City.Province.Name
                },
                Source = new AddressDto
                {
                    CityId = item.Order.Source.CityId,
                    CityName = item.Order.Source.City.Name,
                    ProvinceId = item.Order.Source.City.ProvinceId,
                    ProvinceName = item.Order.Source.City.Province.Name
                },
                WaybillCode = item.WaybillCode,
                PaymentDeadlineExpiredTime = item.PaymentDeadlineExpiredTime
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);
    }
}