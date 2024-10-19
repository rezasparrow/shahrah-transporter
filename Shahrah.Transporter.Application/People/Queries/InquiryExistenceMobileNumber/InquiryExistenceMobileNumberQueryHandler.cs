using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Queries.InquiryExistenceMobileNumber;

public class InquiryExistenceMobileNumberQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<InquiryExistenceMobileNumberQuery, bool>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<bool> Handle(InquiryExistenceMobileNumberQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .SingleOrDefaultAsync(p => p.MobileNumber == request.MobileNumber, cancellationToken);

        return person != null;
    }
}