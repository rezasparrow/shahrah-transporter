using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Scheduling;

namespace Shahrah.Transporter.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(ApplicationDbContext dbContext, QuartzMigrator quartzMigrator)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly QuartzMigrator _quartzMigrator = quartzMigrator;
    private static readonly object MigrationSyncRoot = new();

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