namespace Shahrah.Transporter.Application.Payments.Models
{
    public class VerifyPaymentResultDto
    {
        public long TrackingNumber { get; set; }
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }
}