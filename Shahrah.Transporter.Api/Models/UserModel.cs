using Shahrah.Transporter.Domain.Enums;
using System;

namespace Shahrah.Transporter.Api.Models;

public class UserModel
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool RegistrationIsComplete { get; set; }
    public string TransporterName { get; set; }
    public string TransporterNationalId { get; set; }
    public bool IsAgentWaitingForConfirmation { get; set; }
    public bool IsSubscribed { get; set; }
    public PersonStatus Status { get; set; }
    public string StatusTitle { get; set; }
}