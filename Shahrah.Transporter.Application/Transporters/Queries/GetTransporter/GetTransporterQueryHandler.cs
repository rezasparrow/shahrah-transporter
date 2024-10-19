using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Transporters.Models;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetTransporter;

public class GetTransporterQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTransporterQuery, TransporterDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<TransporterDto> Handle(GetTransporterQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .Include(person => person.Transporter)
            .ThenInclude(transporter => transporter.City)
            .SingleOrDefaultAsync(person => person.Id == request.PersonId, cancellationToken);

        var transporter = person?.Transporter;
        return transporter != null ? new TransporterDto(transporter) : null;
    }
}