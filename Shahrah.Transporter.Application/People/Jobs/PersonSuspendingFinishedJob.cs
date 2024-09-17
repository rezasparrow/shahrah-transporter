using Shahrah.Framework.Scheduling;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.People.Jobs;

public class PersonSuspendingFinishedJob : DelayedJob
{
    private readonly IPersonService _personService;
    private readonly IApplicationDbContext _dbContext;

    public PersonSuspendingFinishedJob(IPersonService personService, IApplicationDbContext dbContext)
    {
        _personService = personService;
        _dbContext = dbContext;
    }

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