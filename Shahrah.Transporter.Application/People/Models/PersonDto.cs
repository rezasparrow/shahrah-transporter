using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.People.Models;

public class PersonDto(Person person)
{
    public long Id { get; set; } = person.Id;
    public string FirstName { get; set; } = person.FirstName;
    public string LastName { get; set; } = person.LastName;
    public string NationalCode { get; set; } = person.NationalCode;
    public DateTime? BirthDate { get; set; } = person.BirthDate;
    public string MobileNumber { get; set; } = person.MobileNumber;
    public PersonStatus Status { get; set; } = person.Status;
    public string StatusTitle => Status.GetDisplayName();
    public AgentRegistrationStatus AgentRegistrationStatus { get; set; } = person.AgentRegistrationStatus;
    public string AgentRegistrationStatusTitle => AgentRegistrationStatus.GetDisplayName();
}