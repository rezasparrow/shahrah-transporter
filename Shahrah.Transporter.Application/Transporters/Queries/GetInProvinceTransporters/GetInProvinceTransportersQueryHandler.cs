using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shahrah.Transporter.Application.Transporters.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetInProvinceTransporters;

public class GetInProvinceTransportersQueryHandler : IRequestHandler<GetInProvinceTransportersQuery, IEnumerable<Person>>
{
    private readonly ITransporterService _transporterService;

    public GetInProvinceTransportersQueryHandler(ITransporterService transporterService)
    {
        _transporterService = transporterService;
    }

    public async Task<IEnumerable<Person>> Handle(GetInProvinceTransportersQuery request, CancellationToken cancellationToken)
    {
        return await _transporterService.GetActivePersonsByTransportersLatLong(request.Latitude, request.Longitude);
    }
}