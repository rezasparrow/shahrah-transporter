using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Drivers.Models;
using Shahrah.Transporter.Application.OrderItems.Models;
using Shahrah.Transporter.Domain.Enums;
using Shahrah.Transporter.Domain.GraphQL;
using Shahrah.Transporter.Domain.Models.DataTransferObjects;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetPaidOrderItems;

public class GetPaidOrderItemsQueryHandler : IRequestHandler<GetPaidOrderItemsQuery, IEnumerable<PaidOrderItemDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IReportService _reportService;

    public GetPaidOrderItemsQueryHandler(IApplicationDbContext dbContext, IReportService reportService)
    {
        _dbContext = dbContext;
        _reportService = reportService;
    }

    public async Task<IEnumerable<PaidOrderItemDto>> Handle(GetPaidOrderItemsQuery request, CancellationToken cancellationToken)
    {
        var paidOrderItems= await _dbContext.OrderItems
            .Include(x => x.Order).ThenInclude(x => x.Source).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Destination).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Load)
            .Include(x => x.Payment)
            .Where(q => q.Order.PersonId == request.PersonId && q.Status == OrderItemStatus.WaitingForLoading)
            .Select(item => new PaidOrderItemDto
            {
                Id = item.Id,
                CorrelationId = item.Order.CorrelationId,
                Value = item.Order.Value,
                LoadTitle = item.Order.Load.Title,
                LoadDescrption = item.Order.LoadDescription,
                AcceptedPrice = item.OfferedPriced,
                OrderId = item.OrderId,
                CreatedDate = item.CreatedDate,
                Status = item.Status,
                StatusTitle = item.Status.GetDisplayName(),
                PaidAmount = item.PaidAmount,
                PaymentDate = item.PaymentDate,
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
                VehicleSmartCardNumber = item.VehicleSmartCardNumber,
                TrackingNumber = item.Payment == null ? 0 : item.Payment.TrackingNumber,
                PaymentDeadlineExpiredTime = item.PaymentDeadlineExpiredTime
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);

        await ApplyReportData(paidOrderItems);

        return paidOrderItems;
    }

    private async Task ApplyReportData(List<PaidOrderItemDto> paidOrderItems)
    {
        if (!paidOrderItems.Any()) return;
        var reportData = await _reportService.PaidOrderItemReportData(paidOrderItems.Select(t => t.CorrelationId).ToArray());
        reportData?.ForEach(reportData =>
        {

            var paidOrderItem = paidOrderItems.FirstOrDefault(t => t.CorrelationId == reportData.Id);
            if (paidOrderItem == null)
                return;

            var driver = reportData.Items?.FirstOrDefault(t => t.Id == paidOrderItem.Id)?.Driver;
            if (driver != null)
                paidOrderItem.Driver = new DriverDto
                {
                    Id = driver.Id,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    MobileNumber = driver.MobileNumber,
                    NationalCode=driver.NationalCode,
                };
        });
    }
}