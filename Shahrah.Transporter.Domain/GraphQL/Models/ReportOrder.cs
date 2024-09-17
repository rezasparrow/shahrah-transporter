using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shahrah.Transporter.Domain.GraphQL.Models
{

    public class ReportOrderListModel
    {
        public List<ReportOrder> Orders { get; set; }
    }

    public class ReportOrder
    {
        public int OrderId { get; set; }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int LoadId { get; set; }
        public string LoadTitle { get; set; }
        public int PackingId { get; set; }
        public string PackingType { get; set; }
        public string PackageDescription { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
        public decimal Value { get; set; }
        public bool IsWeighStationRequire { get; set; }
        public decimal MinimumOfferPrice { get; set; }
        public decimal MaximumOfferPrice { get; set; }
        public decimal? TransporterOfferPrice { get; set; }
        public decimal? SenderOfferPrice { get; set; }
        public bool IsSpecialOffer { get; set; }
        public int VehicleQuantity { get; set; }
        public DateTime SendingDate { get; set; }
        public DateTime LoadingDate { get; set; }
        public ReportOrderStatus Status { get; set; }
        public ReportOrderLocation Source { get; set; }
        public ReportOrderLocation Destination { get; set; }
        public List<ReportOrderItem> Items { get; set; }
        public List<ReportOrderOption> Options { get; set; }
        public int? TransporterId { get; set; }
        public ReporTransporter Transporter { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public int TruckId { get; set; }
        public int? SenderRequestId { get; set; }
        public string SenderName { get; set; }
        public string SenderMobileNumber { get; set; }
        public long? SenderUserId { get; set; }
        public List<ReportOrderOptionItem> OrderOptionItems { get; set; }
        public List<ReportPersonOrder> Receivers { get; set; }
        public long? PersonId { get; set; }
        public ReportOrderTransporterPerson TransporterPerson { get; set; }
        public string LoadDescription { get; set; }
        public string SourceWeighStationAddress { get; set; }
        public string DestinationWeighStationAddress { get; set; }
        public string LoadReceiverFirstName { get; set; }
        public string LoadReceiverLastName { get; set; }
        public string LoadReceiverMobileNumber { get; set; }
        public DateTime ClosedDateTime { get; set; }
        public DateTime PendDateTime { get; set; }
        public DateTime ProgressedDateTime { get; set; }
        public DateTime RegisteredDateTime { get; set; }
        public DateTime TransporterAcceptedSenderOfferPriceDateTime { get; set; }
        public DateTime TransporterOfferPriceDateTime { get; set; }
        public DateTime TransporterRegisteredSenderOrderDateTime { get; set; }
    }

    public class ReportOrderLocation
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }

    public class ReportOrderTransporterPerson
    {
        public long PersonId { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonNationalCode { get; set; }
        public string PersonMobileNumber { get; set; }
        public ReportPersonTypeEnum PersonType { get; set; }
        public long TransporterId { get; set; }
        public string TransporterName { get; set; }
    }

    public class ReportOrderItem
    {
        public int Id { get; set; }
        public decimal OfferedPriced { get; set; }
        public ReportOrderItemStatus Status { get; set; }
        public bool IsLoadingConfirmed { get; set; }
        public bool IsLoadingConfirmedBySender { get; set; }
        public bool IsLoadingConfirmedByDriver { get; set; }
        public DateTime LoadingConfirmationDateTime { get; set; }
        public string WaybillCode { get; set; }
        public bool IsTripEnded { get; set; }
        public bool IsTripEndedBySender { get; set; }
        public bool IsTripEndedByDriver { get; set; }
        public DateTime EndTripDateTime { get; set; }
        public ReportOrderDriver Driver { get; set; }
        public int BidId { get; set; }
        public ReportOrderItemPayment Payment { get; set; }
        public decimal? PaidAmount { get; set; }
        public string SuggesterTransporterName { get; set; }
        public string SuggesterTransporterPhoneNumber { get; set; }
        public string PayerTransporterAgentFirstName { get; set; }
        public string PayerTransporterAgentLastName { get; set; }
        public string PayerTransporterAgentMobileNumber { get; set; }
        public DateTime AuctionClosedDateTime { get; set; }
        public DateTime BidPlacedOnPendingAuctionDateTime { get; set; }
        public DateTime AttemptToPayDateTime { get; set; }
        public DateTime CanceledDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LoadingConfirmedDateTime { get; set; }
        public DateTime? PaidDateTime { get; set; }
        public DateTime PaymentTimeExpiredDateTime { get; set; }
        public DateTime PendingPaymentDateTime { get; set; }
        public DateTime TechnicalyConfirmedDateTime { get; set; }
        public DateTime TripEndedDateTime { get; set; }
        public DateTime WaybillCodeRegisteredDateTime { get; set; }
    }

    public class ReportOrderDriver
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseExpirationDate { get; set; }
        public ReportOrderVehicle Vehicle { get; set; }
    }

    public class ReportOrderVehicle
    {
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerNationalCode { get; set; }
        public string PlateNumber { get; set; }
        public string VIN { get; set; }
        public string SmartCardNumber { get; set; }
        public DateTime SmartCardExpirationDate { get; set; }
        public ReportOrderTruck Truck { get; set; }
        public float NetLoadingCapacity { get; set; }
        public float GrossLoadingCapacity { get; set; }
        public ICollection<ReportOrderOption> Options { get; set; }
    }

    public class ReportOrderTruck
    {
        public string Title { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double MinLoadWeight { get; set; }
        public double? MaxLoadWeight { get; set; }
    }

    public class ReportOrderOption
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ReporTransporter
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ReportOrderOptionItem
    {
        public int OptionItemId { get; set; }
        public string OptionItemValue { get; set; }
        public int OptionId { get; set; }
        public string OptionTitle { get; set; }
        public ReportOptionTypeEnum Type { get; set; }
    }

    public enum ReportOrderStatus
    {
        [Display(Name = "ثبت شده")]
        Registered = 0,
        [Display(Name = "در انتظار تایید قیمت پیشنهادی")]
        WaitingForConfirmOfferedPrice = 10,
        [Display(Name = "تعیین قیمت نهایی شده")]
        PriceFinalized = 20,
        [Display(Name = "در حال جستجو")]
        Searching = 30,
        [Display(Name = "در حال انتظار")]
        Pending = 40,
        [Display(Name = "در حال انجام")]
        InProgress = 50,
        [Display(Name = "بسته شده")]
        Closed = 60,
    }

    public enum ReportPersonTypeEnum : byte
    {
        Owner = 0,
        Agent = 10
    }

    public enum ReportOrderItemStatus
    {
        [Display(Name = "")]
        Initialize = 0,

        [Display(Name = "عدم پیدا شدن راننده")]
        DriverNotFound = 10,

        [Display(Name = "عدم پاسخ راننده")]
        DriverNotRespond = 20,

        [Display(Name = "در حال جستجو")]
        PendingForFindDriver = 30,

        [Display(Name = "در انتظار پرداخت")]
        PendingPayment = 40,

        [Display(Name = "اقدام به پرداخت")]
        AttemptToPay = 50,

        [Display(Name = "در انتظار بارگیری")]
        WaitingForLoading = 60,

        [Display(Name = "در انتظار تایید فنی")]
        WaitingForTechnicalConfirmation = 70,

        [Display(Name = "در انتظار صدور بارنامه")]
        WaitingForWaybillRegisteration = 80,

        [Display(Name = "شروع سفر")]
        TripStarting = 90,

        [Display(Name = "پایان سفر")]
        TripEnded = 100,

        [Display(Name = "لغو شده")]
        Canceled = 300,
    }

    public enum ReportOptionTypeEnum
    {
        Single = 1,
        Multiple = 2
    }
    public class ReportPersonOrder
    {
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public decimal? OfferedPrice { get; set; }
    }

    public class ReportOrderItemPayment
    {
        public long TrackingNumber { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }

        public string TransactionCode { get; set; }

        public string GatewayName { get; set; }

        public string GatewayAccountName { get; set; }
        public string Message { get; set; }
    }
}
