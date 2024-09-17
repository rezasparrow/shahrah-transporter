namespace Shahrah.Transporter.Api.Models;

public class SendOtpRequestToIdentityModel
{
    public string MobileNumber { get; set; }
    public string ClientName { get; set; }
    public string ClientSecret { get; set; }
}

public class SendOtpToChangeMobileRequestToIdentityModel
{
    public string MobileNumber { get; set; }
    public string NewMobileNumber { get; set; }
    public string ClientName { get; set; }
    public string ClientSecret { get; set; }
}

public class ChangeMobileNumberRequestToIdentityModel
{
    public string MobileNumber { get; set; }
    public string NewMobileNumber { get; set; }
    public string ClientName { get; set; }
    public string Token { get; set; }
}