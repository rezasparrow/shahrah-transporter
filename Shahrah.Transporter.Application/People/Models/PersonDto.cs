using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Domain.Entities;
using System;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.People.Models;

public class PersonDto
{
    public PersonDto(Person person)
    {
        Id = person.Id;
        FirstName = person.FirstName;
        LastName = person.LastName;
        NationalCode = person.NationalCode;
        BirthDate = person.BirthDate;
        MobileNumber = person.MobileNumber;
        Status = person.Status;
        AgentRegistrationStatus = person.AgentRegistrationStatus;
    }

    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public string MobileNumber { get; set; }
    public PersonStatus Status { get; set; }
    public string StatusTitle => Status.GetDisplayName();
    public AgentRegistrationStatus AgentRegistrationStatus { get; set; }
    public string AgentRegistrationStatusTitle => AgentRegistrationStatus.GetDisplayName();
}