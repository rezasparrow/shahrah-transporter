namespace Shahrah.Transporter.Application.OrderItems.Models
{
    public class TransporterPersonDto
    {
        public long PersonId { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonNationalCode { get; set; }
        public string PersonMobileNumber { get; set; }
        public long TransporterId { get; set; }
        public string TransporterName { get; set; }
    }
}
