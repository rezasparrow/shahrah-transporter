using System;

namespace Shahrah.Transporter.Application.People.Models;

public class RegisterPersonDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public DateTime BirthDate { get; set; }
    public string MobileNumber { get; set; }
}