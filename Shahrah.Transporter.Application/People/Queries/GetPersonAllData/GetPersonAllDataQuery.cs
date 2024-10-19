using MediatR;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.People.Queries.GetPersonAllData;

public class GetPersonAllDataQuery(string mobileNumber) : IRequest<Person>
{
    public string MobileNumber { get; } = mobileNumber;
}