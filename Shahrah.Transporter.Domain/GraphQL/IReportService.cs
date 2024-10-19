using Shahrah.Transporter.Domain.GraphQL.Models;

namespace Shahrah.Transporter.Domain.GraphQL;

public interface IReportService
{
    Task<List<ReportOrder>> GetOrderItems(Guid[] guids);
    Task<List<ReportOrder>> GetTripEndedReportData(Guid[] guids);

    Task<List<ReportOrder>> PaidOrderItemReportData(Guid[] guids);
}