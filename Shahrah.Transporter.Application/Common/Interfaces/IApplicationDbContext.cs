using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shahrah.Transporter.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Person> People { get; }
    DbSet<Domain.Entities.Transporter> Transporters { get; }
    DbSet<Province> Provinces { get; }
    DbSet<City> Cities { get; }
    DbSet<CityArea> Areas { get; }
    DbSet<Option> Options { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<VehicleOptionItem> VehicleOptionItems { get; }
    DbSet<OptionItem> OptionItems { get; }
    DbSet<Truck> Trucks { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<OrderOptionItem> OrderOptionItems { get; }
    DbSet<Load> Loads { get; }
    DbSet<Package> Packages { get; }
    DbSet<Address> Addresses { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Plan> Plans { get; }
    DbSet<Subscription> Subscriptions { get; }
    DbSet<PersonOrder> PersonOrders { get; }
    DbSet<FinancialTransaction> FinancialTransactions { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IDbContextTransaction BeginTransaction();

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}