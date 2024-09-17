using Shahrah.Framework.Events;
using Shahrah.Framework.Models;
using Shahrah.Transporter.Application.Orders.Services.Interfaces;
using SlimMessageBus;
using System.Linq;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Application.Orders.EventPublishers;

public class TransporterRegisteredSenderOrderEventPublisher
{
    private readonly IMessageBus _messageBus;
    private readonly IOrderQueryService _orderQueryService;

    public TransporterRegisteredSenderOrderEventPublisher(IMessageBus messageBus, IOrderQueryService orderQueryService)
    {
        _messageBus = messageBus;
        _orderQueryService = orderQueryService;
    }

    public async Task Publish(int orderId, long? personId)
    {
        var dbOrder = await _orderQueryService.GetOrderWithAllDataAsync(personId, orderId);
        var orderRegistered = new TransporterRegisteredSenderOrderEvent(dbOrder.CorrelationId)
        {
            Id = dbOrder.Id,
            TransporterPerson = dbOrder.Person != null ? new TransporterPerson
            {
                TransporterName = dbOrder.Person.Transporter.Name,
                TransporterId = dbOrder.Person.Transporter.Id,
                PersonId = dbOrder.Person.Id,
                PersonFirstName = dbOrder.Person.FirstName,
                PersonLastName = dbOrder.Person.LastName,
                PersonNationalCode = dbOrder.Person.NationalCode,
                PersonMobileNumber = dbOrder.Person.MobileNumber,
                PersonType = (PersonTypeEnum)dbOrder.Person.PersonType
            } : null,
            TransporterOfferPrice = dbOrder.TransporterOfferPrice,
            IsSpecialOffer = dbOrder.IsSpecialOffer,
            SendingDate = dbOrder.SendingDate,
            Receivers = dbOrder.Receivers.Select(x => new PersonOrder
            {
                PersonId = x.PersonId,
                PersonName = $"{x.Person.FirstName} {x.Person.LastName}",
                OfferedPrice = x.OfferedPrice
            }).ToList()
        };

        await _messageBus.Publish(orderRegistered);
    }
}