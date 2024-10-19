using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.People.Commands.AgentAdd;
using Shahrah.Transporter.Application.People.Commands.AgentEdit;
using Shahrah.Transporter.Application.People.Commands.DeleteAgent;
using Shahrah.Transporter.Application.People.Jobs;
using Shahrah.Transporter.Application.People.Services.Interfaces;
using Shahrah.Transporter.Domain.Entities;
using Shahrah.Transporter.Domain.Enums;

namespace Shahrah.Transporter.Application.People.Services;

public class PersonService(IApplicationDbContext dbContext, IIdentityServerService identityServerService, IJobScheduler jobScheduler, AppSettings appSettings, INotificationService notificationService) : IPersonService
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IIdentityServerService _identityServerService = identityServerService;
    private readonly IJobScheduler _jobScheduler = jobScheduler;
    private readonly AppSettings _appSettings = appSettings;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<bool> HasSubscription(long personId)
    {
        if (_appSettings.IsAppFree)
            return true;

        return await _dbContext.Subscriptions.AnyAsync(s => s.PersonId == personId
                                                            && s.Status == SubscriptionStatus.Paid
                                                            && s.ExpirationDate > DateTime.Now);
    }

    public async Task<decimal> GetCashBalance(long personId)
    {
        return await _dbContext.FinancialTransactions
             .Where(t => t.PersonId == personId)
             .Select(t => t.TransactionType == FinancialTransactionType.Debit ? -1 * t.Amount : t.Amount)
             .SumAsync();
    }

    public async Task Activate(long personId)
    {
        var person =
            await _dbContext.People.SingleOrDefaultAsync(x => x.Id == personId);

        if (person == null)
            return;

        person.Status = PersonStatus.Active;
        _dbContext.People.Update(person);
        await _dbContext.SaveChangesAsync();
        await _notificationService.SendToPerson(personId, x => x.UserActivated());
    }

    public async Task AgentAccept(long agentId, DateTime birthDate, string firstName, string lastName,
        string nationalCode, CancellationToken cancellationToken = default)
    {
        var nominatedAgent = await _dbContext.People.SingleOrDefaultAsync(
            t => t.Id == agentId && t.AgentRegistrationStatus == AgentRegistrationStatus.Pending,
            cancellationToken);

        if (nominatedAgent == null)
            throw new Exception("No nominated agent found.");

        nominatedAgent.FirstName = firstName;
        nominatedAgent.LastName = lastName;
        nominatedAgent.NationalCode = nationalCode;
        nominatedAgent.BirthDate = birthDate;
        nominatedAgent.AgentRegistrationStatus = AgentRegistrationStatus.Registered;
        nominatedAgent.Status = PersonStatus.Active;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAgent(AgentAddCommand request, CancellationToken cancellationToken = default)
    {
        // TODO: Javad Rasouli >> این چند خط جاهای دیگه هم استفاده شده، امکان اینکه در قالب یه Attribute نوشته بشه یا هر چیز دیگه وجود داره.
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        if (await PersonIsAllreadyRegistered(request.MobileNumber, request.NationalCode))
            throw new DomainException(ErrorMessageResource.AgentAlreadyRegistered);

        var personId = await _identityServerService.AddUser(request.MobileNumber);

        var agent = new Person
        {
            Id = personId,
            TransporterId = person.TransporterId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalCode = request.NationalCode,
            MobileNumber = request.MobileNumber,
            AgentRegistrationStatus = AgentRegistrationStatus.Pending,
            Status = PersonStatus.DeActive,
            PersonType = PersonTypeEnum.Agent
        };
        await _dbContext.People.AddAsync(agent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAgent(AgentEditCommand request, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var agnet = await _dbContext.People.SingleOrDefaultAsync(
            t => t.TransporterId == person.TransporterId && t.Id == request.AgentId,
            cancellationToken);

        if (agnet == null) throw new Exception("Transporter agent not found.");

        agnet.FirstName = request.FirstName;
        agnet.LastName = request.LastName;
        agnet.NationalCode = request.NationalCode;

        _dbContext.People.Update(agnet);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AgentNotAccept(long agentId, CancellationToken cancellationToken = default)
    {
        var agent = await _dbContext.People
            .SingleAsync(person => person.Id == agentId, cancellationToken);

        if (agent == null) throw new Exception("agent not found.");

        if (!await _identityServerService.RemoveUser(agent.MobileNumber))
            throw new Exception("Can not remove user from identity");

        agent.AgentRegistrationStatus = AgentRegistrationStatus.Revoked;

        _dbContext.People.Update(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeMobileNumber(long personId, string mobileNumber, string otp, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == personId, cancellationToken);

        if (person == null)
            throw new Exception("person not found");

        var changeMobileNumberInIdentityResult = await _identityServerService.ChangeMobileNumber(person.MobileNumber, mobileNumber, otp);
        if (!changeMobileNumberInIdentityResult)
            throw new DomainException(ErrorMessageResource.CodeIsInvalid);

        person.MobileNumber = mobileNumber;

        _dbContext.People.Update(person);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CloseAccount(long personId, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == personId, cancellationToken);

        switch (person.PersonType)
        {
            case PersonTypeEnum.Owner:
                RemoveTransporter(person.TransporterId);
                break;

            case PersonTypeEnum.Agent:
                RemoveAgent(person.Id);
                break;

            default:
                throw new NotImplementedException();
        }

        var result = await _identityServerService.RemoveUser(person.MobileNumber);
        if (result == false)
            throw new DomainException(ErrorMessageResource.InvalidOperationError);
    }

    public async Task DeleteAgent(DeleteAgentCommand request, CancellationToken cancellationToken = default)
    {
        var person = await _dbContext.People
            .SingleAsync(person => person.Id == request.PersonId, cancellationToken);

        if (person.PersonType != PersonTypeEnum.Owner)
            throw new DomainException(ErrorMessageResource.AuthorizationFailed);

        var agnet = await _dbContext.People.SingleOrDefaultAsync(
            t => t.TransporterId == person.TransporterId && t.Id == request.AgentId,
            cancellationToken);

        if (agnet == null) throw new Exception("Transporter agent not found.");

        if (!await _identityServerService.RemoveUser(agnet.MobileNumber))
            throw new Exception("Can not remove user from identity");

        _dbContext.People.Remove(agnet);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Register(Person realPerson, CancellationToken cancellationToken)
    {
        var person =
            await _dbContext.People.SingleOrDefaultAsync(
                r => r.MobileNumber == realPerson.MobileNumber, cancellationToken);

        if (person != null)
            throw new DomainException(ErrorMessageResource.UserAlreadyRegistered);

        await _dbContext.People.AddAsync(realPerson, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Suspend(long personId, CancellationToken cancellationToken)
    {
        var person =
            await _dbContext.People.SingleOrDefaultAsync(x => x.Id == personId, cancellationToken);

        if (person == null)
            return;

        person.Status = PersonStatus.Suspended;
        _dbContext.People.Update(person);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _jobScheduler.ScheduleAt<PersonSuspendingFinishedJob>(builder =>
           builder.WithIdentifier(person.Id.ToString())
               .WithSeconds(_appSettings.SuspendingPersonDurationBySecond)
               .WithData(new Dictionary<string, string> { { "PersonId", person.Id.ToString() } }));
    }

    private async Task<bool> PersonIsAllreadyRegistered(string mobileNumber, string nationalCode)
    {
        return await _dbContext.People.AnyAsync(
            t => (t.MobileNumber == mobileNumber || t.NationalCode == nationalCode) &&
                t.AgentRegistrationStatus != AgentRegistrationStatus.Revoked);
    }

    private void RemoveTransporter(long transporterId)
    {
        // TODO: Javad Rasouli >> بستن اکانت مشکل داره
        throw new NotImplementedException();
        //var vehicles = await _unitOfWork.Vehicles.GetVehicles(transporterId);

        //foreach (var vehicle in vehicles)
        //    await _unitOfWork.Vehicles.RemoveVehicle(vehicle.Id);

        //await _unitOfWork.Subscriptions.DeleteTransporterSubscriptions(transporterId);

        //await _unitOfWork.Payments.DeleteTransporterPayments(transporterId);

        //await _unitOfWork.Orders.DeleteAllTransporterOrders(transporterId);

        //await _unitOfWork.People.RemovePersonWithTransporter(transporterId);

        //await _unitOfWork.Transporters.DeleteAsync(transporterId);

        //// TODO: Javad Rasouli >> احتمالا ایجنتهای یک ترنسپورتر باید حذف شود

        //await _dbContext.SaveChangesAsync(cancellationToken);

        //return Unit.Value;
    }

    private void RemoveAgent(long personId)
    {
        // TODO: Javad Rasouli >> پیاده سازی بستن اکانت ایجنت
        throw new NotImplementedException();
    }
}