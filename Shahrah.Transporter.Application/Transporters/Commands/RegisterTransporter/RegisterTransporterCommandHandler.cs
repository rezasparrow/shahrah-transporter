using MediatR;
using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Transporters.Commands.RegisterTransporter;

internal class RegisterTransporterCommandHandler : IRequestHandler<RegisterTransporterCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterTransporterCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(RegisterTransporterCommand request, CancellationToken cancellationToken)
    {
        var isPersonAlreadyExist = await _dbContext.People.SingleOrDefaultAsync(
            r => r.MobileNumber == request.Person.MobileNumber &&
                 r.AgentRegistrationStatus != AgentRegistrationStatus.Revoked, cancellationToken) != null;

        if (isPersonAlreadyExist)
            throw new DomainException(ErrorMessageResource.UserAlreadyRegistered);

        var transporterEntity = new Domain.Entities.Transporter
        {
            Name = request.Transporter.Name,
            NationalId = request.Transporter.NationalId,
            LicenseSerialNumber = request.Transporter.LicenseSerialNumber,
            LicenseExpirationDate = request.Transporter.LicenseExpirationDate,
            CityId = request.Transporter.CityId,
            Address = request.Transporter.Address,
            PostalCode = request.Transporter.PostalCode,
            PhoneNumber = request.Transporter.PhoneNumber,
            ActivityZone = request.Transporter.TransporterActivityZone,
            Latitude = request.Transporter.Latitude,
            Longitude = request.Transporter.Longitude,
            People = new List<Person>
            {
                new Person
                {
                    Id = request.Person.Id,
                    FirstName = request.Person.FirstName,
                    LastName = request.Person.LastName,
                    NationalCode = request.Person.NationalCode,
                    BirthDate = request.Person.BirthDate.Date,
                    MobileNumber = request.Person.MobileNumber,
                    PersonType = PersonTypeEnum.Owner
                }
            }
        };

        await _dbContext.Transporters.AddAsync(transporterEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}