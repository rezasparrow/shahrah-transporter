using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.GraphQL;
using Shahrah.Transporter.Infrastructure.GraphQL.Services;
using Shahrah.Transporter.Infrastructure.Persistence;
using Shahrah.Transporter.Infrastructure.Persistence.Interceptors;

namespace Shahrah.Transporter.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseSqlServer(configuration.GetConnectionString("Default"), sqlServerDbContextOptionsBuilder =>
                    sqlServerDbContextOptionsBuilder.UseNetTopologySuite()
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .EnableSensitiveDataLogging());

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<SoftDeletableEntitySaveChangesInterceptor>();
        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }

    public static void AddGraphQlClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(configuration["GraphQLURI"], new NewtonsoftJsonSerializer()));
        services.AddScoped<IReportService, ReportService>();
    }
}