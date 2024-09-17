using Shahrah.Framework.Models;

namespace Shahrah.Transporter.Application.Common.Models;

public class AppSettings
{
    public string Name { get; set; }
    public int AuctionDurationBySecond { get; set; }
    public int PendingOrderDurationBySecond { get; set; }
    public int PaymentDuration { get; set; }
    public decimal ShahrahCommissionFromFindingDriver { get; set; }
    public string ClientAppUrl { get; set; }
    public string PaymentUrl { get; set; }
    public int PaymentInBankGatewayDuration { get; set; }
    public string AppDownloadLink { get; set; }
    public string ReportBaseUrl { get; set; }
    public byte SuspendingPersonDurationBySecond { get; set; }
    public bool IsAppFree { get; set; }

    public IdentityServerModel IdentityServer { get; set; }
}