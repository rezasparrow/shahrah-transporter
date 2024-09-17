using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Application.Common.Interfaces;
using SlimMessageBus;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.EventPublishers;

public class WinnerTransporterSpecifiedEventPublisher
{
    private readonly IMessageBus _messageBus;
    private readonly IApplicationDbContext _dbContext;

    public WinnerTransporterSpecifiedEventPublisher(IMessageBus messageBus, IApplicationDbContext dbContext)
    {
        _messageBus = messageBus;
        _dbContext = dbContext;
    }

    public async Task Publish(int orderId)
    {
        var dbOrder = _dbContext.Orders
            .Include(q => q.Person).ThenInclude(q => q.Transporter)
            .Single(order => order.Id == orderId);

        var orderRegistered = new WinnerTransporterSpecifiedEvent(dbOrder.CorrelationId)
        {
            TransporterPerson = new TransporterPerson
            {
                TransporterName = dbOrder.Person.Transporter.Name,
                TransporterId = dbOrder.Person.Transporter.Id,
                PersonId = dbOrder.Person.Id,
                PersonFirstName = dbOrder.Person.FirstName,
                PersonLastName = dbOrder.Person.LastName,
                PersonNationalCode = dbOrder.Person.NationalCode,
                PersonMobileNumber = dbOrder.Person.MobileNumber,
                PersonType = (PersonTypeEnum)dbOrder.Person.PersonType
            }
        };

        await _messageBus.Publish(orderRegistered);
    }
}