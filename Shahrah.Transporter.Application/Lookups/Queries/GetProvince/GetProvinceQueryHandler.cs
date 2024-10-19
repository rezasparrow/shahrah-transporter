using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Lookups.Models;

namespace Shahrah.Transporter.Application.Lookups.Queries.GetProvince;

public class GetProvinceQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetProvinceQuery, ProvinceDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<ProvinceDto> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
    {
        var province = await _dbContext.Provinces.SingleAsync(x => x.Id == request.ProvinceId, cancellationToken);
        return new ProvinceDto { Id = province.Id, Name = province.Name };
    }
}