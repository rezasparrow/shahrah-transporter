using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class AgentAcceptModel
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string NationalCode { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
}