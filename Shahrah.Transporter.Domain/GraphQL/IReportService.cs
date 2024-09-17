using Shahrah.Transporter.Domain.GraphQL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Domain.GraphQL;

public interface IReportService
{
    Task<List<ReportOrder>> GetOrderItems(Guid[] guids);
    Task<List<ReportOrder>> GetTripEndedReportData(Guid[] guids);

    Task<List<ReportOrder>> PaidOrderItemReportData(Guid[] guids);
}