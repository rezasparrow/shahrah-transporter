using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Transporters.Services.Interfaces;

public interface ITransporterService
{
    Task<IEnumerable<Person>> GetActivePersonsByTransportersLatLong(double x, double y);
}