using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Drivers.Models;
using Shahrah.Transporter.Application.OrderItems.Models;
using Shahrah.Transporter.Domain.Enums;
using Shahrah.Transporter.Domain.GraphQL;
using Shahrah.Transporter.Domain.Models.DataTransferObjects;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetTripEndedOrderItems;

public class GetTripEndedOrderItemsQueryHandler(IApplicationDbContext dbContext,
    IReportService reportService) : IRequestHandler<GetTripEndedOrderItemsQuery, IEnumerable<TripEndedOrderItemDto>>
{
    private readonly IReportService _reportService = reportService;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<TripEndedOrderItemDto>> Handle(GetTripEndedOrderItemsQuery request, CancellationToken cancellationToken)
    {

        var tripEndedData = await _dbContext.OrderItems
            .Include(x => x.Order).ThenInclude(x => x.Source).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Destination).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Load)
            .Where(q => q.Order.PersonId == request.PersonId && q.Status == OrderItemStatus.TripEnded)
            .Select(item => new TripEndedOrderItemDto
            {
                Id = item.Id,
                CorrelationId = item.Order.CorrelationId,
                Value = item.Order.Value,
                LoadTitle = item.Order.Load.Title,
                LoadDescription = item.Order.LoadDescription,
                AcceptedPrice = item.OfferedPriced,
                OrderId = item.OrderId,
                CreatedDate = item.CreatedDate,
                Status = item.Status,
                StatusTitle = item.Status.GetDisplayName(),
                IsLoadingConfirmed = item.IsLoadingConfirmed,
                IsTripEnded = item.IsTripEnded,
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
                PaidAmount=item.PaidAmount,
                PayemntDate=item.PaymentDate,
                Sender = new SenderDto { },
                Driver = new DriverDto { },
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);

        await ApplyReportData(tripEndedData);

        return tripEndedData;
    }

    private async Task ApplyReportData(List<TripEndedOrderItemDto> tripEndedData)
    {
        if (!tripEndedData.Any()) return;
        var reportData = await _reportService.GetTripEndedReportData(tripEndedData.Select(t => t.CorrelationId).ToArray());
        reportData?.ForEach(reportData =>
        {

            var tirpendedItem = tripEndedData.FirstOrDefault(t => t.CorrelationId == reportData.Id);
            if (tirpendedItem == null)
                return;

            tirpendedItem.Sender = new SenderDto
            {
                FullName = reportData.SenderName,
                MobileNumber = reportData.SenderMobileNumber

            };

            if (reportData.Source != null)
                tirpendedItem.Source = new AddressDto
                {
                    CityId = reportData.Source.CityId,
                    CityName = reportData.Source.CityName,
                    ProvinceId = reportData.Source.ProvinceId,
                    Latitude = reportData.Source.Latitude,
                    Longitude = reportData.Source.Longitude,
                    ProvinceName = reportData.Source.ProvinceName
                };

            if (reportData.Destination != null)
                tirpendedItem.Destination = new AddressDto
                {
                    CityId = reportData.Source.CityId,
                    CityName = reportData.Source.CityName,
                    ProvinceId = reportData.Source.ProvinceId,
                    Latitude = reportData.Source.Latitude,
                    Longitude = reportData.Source.Longitude,
                    ProvinceName = reportData.Source.ProvinceName
                };

            var reportOrderItem = reportData.Items?.FirstOrDefault(t => t.Id == tirpendedItem.Id);
            if (reportOrderItem != null)
            {
                var driver = reportOrderItem?.Driver;
                if (driver != null)
                    tirpendedItem.Driver = new DriverDto
                    {
                        Id = driver.Id,
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        MobileNumber = driver.MobileNumber
                    };

                tirpendedItem.EndTripDateTime = reportOrderItem.EndTripDateTime;
            }

        });
    }

}