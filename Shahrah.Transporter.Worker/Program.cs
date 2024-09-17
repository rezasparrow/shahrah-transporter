using Shahrah.Framework.Extensions;
using Shahrah.Framework.Scheduling;
using SlimMessageBus;
using System.Reflection;

IConfiguration Configuration = null!;
IHostBuilder builder = Host.CreateDefaultBuilder(args);
IHost host = builder
    .ConfigureAppConfiguration((hostContext, configBuilder) =>
        {
            Configuration = configBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();
        })
    .ConfigureServices(services =>
    {
        var jobs = Assembly.Load("Shahrah.Transporter.Application")
                            .GetTypes()
                            .Where(p => typeof(DelayedJob).IsAssignableFrom(p))
                            .ToList();

        services.AddScheduling(Configuration, jobs);
        services.AddSchedulingWorker();
        services.AddSchedulingMigrator(Configuration);
    })
    .UseLogging()
    .Build();

// var scope = host.Services.CreateScope();
// var migrator = scope.ServiceProvider.GetRequiredService<QuartzMigrator>();
// migrator.Migrate();

//host.Services.GetRequiredService<IMessageBus>();

await host.RunAsync();