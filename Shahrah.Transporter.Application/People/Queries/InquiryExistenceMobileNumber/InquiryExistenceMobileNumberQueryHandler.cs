using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;

namespace Shahrah.Transporter.Application.People.Queries.InquiryExistenceMobileNumber;

public class InquiryExistenceMobileNumberQueryHandler : IRequestHandler<InquiryExistenceMobileNumberQuery, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public InquiryExistenceMobileNumberQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(InquiryExistenceMobileNumberQuery request, CancellationToken cancellationToken)
    {
        var person = await _dbContext.People
            .SingleOrDefaultAsync(p => p.MobileNumber == request.MobileNumber, cancellationToken);

        return person != null;
    }
}