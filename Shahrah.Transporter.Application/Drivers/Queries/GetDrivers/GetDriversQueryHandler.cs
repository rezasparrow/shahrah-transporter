using Shahrah.Framework.Requests;
using Shahrah.Framework.Responses;
using SlimMessageBus;

namespace Shahrah.Transporter.Application.Drivers.Queries.GetDrivers;

public class GetDriversQueryHandler(IMessageBus bus) : MediatR.IRequestHandler<GetDriversQuery, Framework.Models.Driver>
{
    private readonly IMessageBus _bus = bus;

    public async Task<Framework.Models.Driver> Handle(GetDriversQuery request, CancellationToken cancellationToken)
    {
        FindDriverRequest driverRequest = new FindDriverRequest(request.NationalCode);
        var response = await _bus.Send<FindDriverResponse, FindDriverRequest>(driverRequest, cancellationToken: cancellationToken);
        return response.Driver;
    }
}