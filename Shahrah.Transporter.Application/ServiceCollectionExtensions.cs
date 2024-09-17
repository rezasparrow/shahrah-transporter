using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shahrah.Transporter.Application.Common.Behaviours;
using Shahrah.Transporter.Application.Drivers.Services;
using Shahrah.Transporter.Application.Drivers.Services.Interfaces;
using Shahrah.Transporter.Application.FinancialTransactions.Services;
using Shahrah.Transporter.Application.FinancialTransactions.Services.Interfaces;
using Shahrah.Transporter.Application.OrderItems.EventPublishers;
using Shahrah.Transporter.Application.OrderItems.Services;
using Shahrah.Transporter.Application.OrderItems.Services.Interfaces;
using Shahrah.Transporter.Application.Orders.EventPublishers;
using Shahrah.Transporter.Application.Orders.Services;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using Shahrah.Transporter.Application.Payments.Services;
using Shahrah.Transporter.Application.Payments.Services.Interfaces;
using Shahrah.Transporter.Application.People.Services;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using Shahrah.Transporter.Application.Transporters.Services;
using Shahrah.Transporter.Application.Transporters.Services.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Services;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;

namespace Shahrah.Transporter.Application;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<IFinancialTransactionService, FinancialTransactionService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IOrderItemPaymentService, OrderItemPaymentService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<ICloseOrderService, CloseOrderService>();
        services.AddScoped<IHandleRegisteredOrderBySenderService, HandleRegisteredOrderBySenderService>();
        services.AddScoped<IOrderPricingService, OrderPricingService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderQueryService, OrderQueryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ITransporterService, TransporterService>();
        services.AddScoped<IVehicleService, VehicleService>();

        services.AddScoped<OrderRegisteredEventPublisher>();
        services.AddScoped<SearchedDriverEventPublisher>();
        services.AddScoped<OrderItemCreatedEventPublisher>();
        services.AddScoped<OrderItemChangeStateEventPublisher>();
        services.AddScoped<TransporterRegisteredSenderOrderEventPublisher>();
        services.AddScoped<WinnerTransporterSpecifiedEventPublisher>();
        services.AddScoped<TransportersVehicleUpdatedEventPublisher>();
        services.AddScoped<OrderItemPaidEventPublisher>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
    }
}