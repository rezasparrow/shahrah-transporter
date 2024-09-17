using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Infrastructure.Persistence.Interceptors;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using OrderOptionItem = Shahrah.Transporter.Domain.Entities.OrderOptionItem;
using Payment = Shahrah.Transporter.Domain.Entities.Payment;
using PersonOrder = Shahrah.Transporter.Domain.Entities.PersonOrder;
using Truck = Shahrah.Transporter.Domain.Entities.Truck;
using Vehicle = Shahrah.Transporter.Domain.Entities.Vehicle;
using VehicleOptionItem = Shahrah.Transporter.Domain.Entities.VehicleOptionItem;

namespace Shahrah.Transporter.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly SoftDeletableEntitySaveChangesInterceptor _softDeletableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
        SoftDeletableEntitySaveChangesInterceptor softDeletableEntitySaveChangesInterceptor)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _softDeletableEntitySaveChangesInterceptor = softDeletableEntitySaveChangesInterceptor;
    }

    public DbSet<Person> People => Set<Person>();

    public DbSet<Domain.Entities.Transporter> Transporters => Set<Domain.Entities.Transporter>();

    public DbSet<Province> Provinces => Set<Province>();

    public DbSet<City> Cities => Set<City>();

    public DbSet<CityArea> Areas => Set<CityArea>();

    public DbSet<Option> Options => Set<Option>();

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    public DbSet<VehicleOptionItem> VehicleOptionItems => Set<VehicleOptionItem>();

    public DbSet<OptionItem> OptionItems => Set<OptionItem>();

    public DbSet<Truck> Trucks => Set<Truck>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<OrderOptionItem> OrderOptionItems => Set<OrderOptionItem>();

    public DbSet<Load> Loads => Set<Load>();

    public DbSet<Package> Packages => Set<Package>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Plan> Plans => Set<Plan>();

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    public DbSet<PersonOrder> PersonOrders => Set<PersonOrder>();

    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();

    public IDbContextTransaction BeginTransaction()
    {
        return Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.NoAction;

        foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType.BaseType == typeof(Entity<>) /*|| e.ClrType.BaseType == typeof(OwnedEntity)*/))
            SetBaseEntityModelConfigMethod.MakeGenericMethod(entity.ClrType).Invoke(this, new object[] { modelBuilder });

        ApplySoftDeleteQuery(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_softDeletableEntitySaveChangesInterceptor);
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    private static void ApplySoftDeleteQuery(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes().Where(entityType => entityType.ClrType.BaseType is { IsGenericType: true } && entityType.ClrType.BaseType
                .GetGenericTypeDefinition() == typeof(Entity<>))
            .ToList().ForEach(entityType =>
            {
                var parameter = Expression.Parameter(entityType.ClrType);

                var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
                if (propertyMethodInfo == null) return;
                var isDeletedProperty =
                    Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

                var compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty,
                    Expression.Constant(false));

                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            });
    }

    private static readonly MethodInfo SetBaseEntityModelConfigMethod = typeof(ApplicationDbContext)
        .GetMethod(nameof(ModelBaseConfiguration), BindingFlags.NonPublic | BindingFlags.Instance);

    private static void ModelBaseConfiguration<T, TKey>(ModelBuilder builder) where T : Entity<TKey>
    {
        builder.Entity<T>().HasKey(e => e.Id);

        builder.Entity<T>().Property<bool>("IsDeleted");
        builder.Entity<T>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

        builder.Entity<T>()
            .Property(e => e.CorrelationId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        builder.Entity<T>().HasIndex(x => x.CorrelationId).IsUnique();
    }
}