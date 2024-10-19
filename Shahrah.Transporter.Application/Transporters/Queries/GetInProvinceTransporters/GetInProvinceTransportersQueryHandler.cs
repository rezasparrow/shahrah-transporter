using MediatR;
using Shahrah.Transporter.Application.Transporters.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Transporters.Queries.GetInProvinceTransporters;

public class GetInProvinceTransportersQueryHandler(ITransporterService transporterService) : IRequestHandler<GetInProvinceTransportersQuery, IEnumerable<Person>>
{
    private readonly ITransporterService _transporterService = transporterService;

    public async Task<IEnumerable<Person>> Handle(GetInProvinceTransportersQuery request, CancellationToken cancellationToken)
    {
        return await _transporterService.GetActivePersonsByTransportersLatLong(request.Latitude, request.Longitude);
    }
}