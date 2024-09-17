using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shahrah.Framework.Extensions;
using Shahrah.Framework.Scheduling;
using System.Linq;
using System.Reflection;

namespace Shahrah.Transporter.Api.Extensions.ServiceCollection
{
    public static class QuartsExtensions
    {
        public static void AddQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            var jobs = Assembly.Load("Shahrah.Transporter.Application")
                    .GetTypes()
                    .Where(p => typeof(DelayedJob).IsAssignableFrom(p))
                    .ToList();

            services.AddScheduling(configuration, jobs);
            services.AddSchedulingWorker();
            services.AddSchedulingMigrator(configuration);
        }
    }
}
