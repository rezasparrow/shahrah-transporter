using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Scheduling;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _dbContext;
    private readonly QuartzMigrator _quartzMigrator;
    private static readonly object MigrationSyncRoot = new();

    public ApplicationDbContextInitialiser(ApplicationDbContext dbContext, QuartzMigrator quartzMigrator)
    {
        _dbContext = dbContext;
        _quartzMigrator = quartzMigrator;
    }

    public async Task InitialiseAsync()
    {
        if (!_dbContext.Database.IsSqlServer())
            return;

        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
            lock (MigrationSyncRoot)
            {
                pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                    _quartzMigrator.Migrate();
                }
            }
    }
}