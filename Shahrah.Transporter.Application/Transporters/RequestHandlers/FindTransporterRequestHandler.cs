using MediatR;
using Shahrah.Framework.Models;
using Shahrah.Framework.Requests;
using Shahrah.Framework.Responses;
using Shahrah.Transporter.Application.Transporters.Queries.GetInProvinceTransporters;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Transporters.RequestHandlers;

public class FindTransporterRequestHandler : SlimMessageBus.IRequestHandler<FindTransporterRequest, FindTransporterResponse>
{
    private readonly IMediator _mediator;

    public FindTransporterRequestHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<FindTransporterResponse> OnHandle(FindTransporterRequest request)
    {
        var peopel = await _mediator.Send(new GetInProvinceTransportersQuery(request.X, request.Y));

        return new FindTransporterResponse
        {
            TransporterPeopel = peopel.Select(person => new TransporterPerson
            {
                PersonFirstName = person.FirstName,
                PersonId = person.Id,
                PersonLastName = person.LastName,
                PersonType = (PersonTypeEnum)person.PersonType,
                PersonMobileNumber = person.MobileNumber,
                PersonNationalCode = person.NationalCode,
                TransporterId = person.TransporterId,
                TransporterName = person.Transporter.Name
            })
        };
    }
}