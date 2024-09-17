using MediatR;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.People.Queries.GetPersonAllData;

public class GetPersonAllDataQuery : IRequest<Person>
{
    public GetPersonAllDataQuery(string mobileNumber)
    {
        MobileNumber = mobileNumber;
    }

    public string MobileNumber { get; }
}