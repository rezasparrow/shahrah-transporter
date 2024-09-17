using Shahrah.Transporter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Transporters.Services.Interfaces;

public interface ITransporterService
{
    Task<IEnumerable<Person>> GetActivePersonsByTransportersLatLong(double x, double y);
}