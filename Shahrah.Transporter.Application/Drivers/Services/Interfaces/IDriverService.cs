using Shahrah.Framework.Responses;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Application.Drivers.Services.Interfaces;

public interface IDriverService
{
    Task<AllocateDriverResponse> SendMessageToDriverForAllocateDriver(int vehicleId, long personId, decimal price,
        Order order, CancellationToken cancellationToken);
}