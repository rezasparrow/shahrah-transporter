using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Drivers.Models;
using Shahrah.Transporter.Application.OrderItems.Models;
using Shahrah.Transporter.Domain.GraphQL;
using Shahrah.Transporter.Domain.Models.DataTransferObjects;

namespace Shahrah.Transporter.Application.OrderItems.Queries.GetOrderItems;

public class GetOrderItemsQueryHandler(IApplicationDbContext dbContext, IReportService reportService) : IRequestHandler<GetOrderItemsQuery, IEnumerable<OrderItemDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IReportService _reportService = reportService;

    public async Task<IEnumerable<OrderItemDto>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
    {
        var orderItems = await _dbContext.OrderItems
            .Include(x => x.Order).ThenInclude(x => x.Source).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Destination).ThenInclude(x => x.City).ThenInclude(x => x.Province)
            .Include(x => x.Order).ThenInclude(x => x.Load)
            .Where(q => q.OrderId == request.OrderId && q.Order.PersonId.HasValue &&
                        q.Order.PersonId == request.PersonId)
            .Select(item => new OrderItemDto
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
                PaymentDeadlineExpiredTime = item.PaymentDeadlineExpiredTime,
                Sender = new SenderDto { }
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);

        await ApplyReportData(orderItems);

        return orderItems;
    }

    private async Task ApplyReportData(List<OrderItemDto> orderItems)
    {
        if (!orderItems.Any()) return;
        var reportOrders = await _reportService.GetOrderItems(orderItems.Select(t => t.CorrelationId).ToArray());

        if (!reportOrders.Any()) return;

        orderItems.ForEach(orderItem =>
        {
            var reportOrder = reportOrders.FirstOrDefault(t => t.Id == orderItem.CorrelationId);

            if (reportOrder.Source != null)
                orderItem.Source = new AddressDto
                {
                    CityId = reportOrder.Source.CityId,
                    CityName = reportOrder.Source.CityName,
                    ProvinceId = reportOrder.Source.ProvinceId,
                    Latitude = reportOrder.Source.Latitude,
                    Longitude = reportOrder.Source.Longitude,
                    ProvinceName = reportOrder.Source.ProvinceName
                };

            if (reportOrder.Destination != null)
                orderItem.Destination = new AddressDto
                {
                    CityId = reportOrder.Source.CityId,
                    CityName = reportOrder.Source.CityName,
                    ProvinceId = reportOrder.Source.ProvinceId,
                    Latitude = reportOrder.Source.Latitude,
                    Longitude = reportOrder.Source.Longitude,
                    ProvinceName = reportOrder.Source.ProvinceName
                };

            if (orderItem.Status >= Domain.Enums.OrderItemStatus.WaitingForLoading)
            {
                if (reportOrder == null) return;
                orderItem.Sender = new SenderDto
                {
                    FullName = reportOrder.SenderName,
                    MobileNumber = reportOrder.SenderMobileNumber
                };

                orderItem.LoadReceiverFirstName = reportOrder.LoadReceiverFirstName;
                orderItem.LoadReceiverLastName = reportOrder.LoadReceiverLastName;
                orderItem.LoadReceiverMobileNumber = reportOrder.LoadReceiverMobileNumber;

                var reportOrderItem = reportOrder?.Items?.FirstOrDefault(t => t.Id == orderItem.Id);
                if (reportOrderItem == null)
                    return;

                if (reportOrderItem.Driver != null)
                {
                    var dirver = reportOrderItem.Driver;
                    orderItem.Driver = new DriverDto
                    {
                        Id = dirver.Id,
                        FirstName = dirver.FirstName,
                        LastName = dirver.LastName,
                        NationalCode = dirver.NationalCode,
                        BirthDate = dirver.BirthDate,
                        MobileNumber = dirver.MobileNumber,
                        DrivingLicenseNumber = dirver.DrivingLicenseNumber,
                        DrivingLicenseExpirationDate = dirver.DrivingLicenseExpirationDate,
                    };
                }

            }
        });

    }
}