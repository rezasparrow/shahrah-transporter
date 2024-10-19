using Shahrah.Framework.Scheduling;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Services.Interfaces;

namespace Shahrah.Transporter.Application.People.Jobs;

public class PersonSuspendingFinishedJob(IPersonService personService, IApplicationDbContext dbContext) : DelayedJob
{
    private readonly IPersonService _personService = personService;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public override async Task RunAsync(Dictionary<string, string> data)
    {
        // TODO: Javad Rasouli >> برای جابها قابلیت بازکردن و بستن خودکار ترنزکشن فراهم شود
        var personId = GetData<long>("PersonId");

        await using var tran = await _dbContext.BeginTransactionAsync();
        try
        {
            await _personService.Activate(personId);
            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
        }
    }
}