using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shahrah.Framework.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Infrastructure.Persistence.Interceptors;

public class SoftDeletableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetSoftDeleteColumns(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetSoftDeleteColumns(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetSoftDeleteColumns(DbContext context)
    {
        if (context == null) return;

        var entriesDeleted = context.ChangeTracker
            .Entries<ISoftDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entityEntry in entriesDeleted)
        {
            entityEntry.Entity.IsDeleted = true;
            entityEntry.Entity.DeletedDate = DateTimeOffset.Now;
            entityEntry.State = EntityState.Modified;
        }
    }
}