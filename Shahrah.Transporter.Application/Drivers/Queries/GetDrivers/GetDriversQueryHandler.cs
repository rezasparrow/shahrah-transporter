using System.Threading;
using System.Threading.Tasks;
using Shahrah.Framework.Requests;
using Shahrah.Framework.Responses;
using SlimMessageBus;

namespace Shahrah.Transporter.Application.Drivers.Queries.GetDrivers;

public class GetDriversQueryHandler : MediatR.IRequestHandler<GetDriversQuery, Framework.Models.Driver>
{
    private readonly IMessageBus _bus;

    public GetDriversQueryHandler(IMessageBus bus)
    {
        _bus = bus;
    }

    public async Task<Framework.Models.Driver> Handle(GetDriversQuery request, CancellationToken cancellationToken)
    {
        var response = await _bus.Send<FindDriverResponse, FindDriverRequest>(new FindDriverRequest(request.NationalCode), cancellationToken: cancellationToken);
        return response.Driver;
    }
}