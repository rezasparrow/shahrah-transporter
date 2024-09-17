using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Domain.Enums;

public enum OrderItemStatus
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