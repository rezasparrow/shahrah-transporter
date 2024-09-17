using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Domain.Enums;

public enum OrderStatus
{
    [Display(Name = "ثبت شده")]
    Registered = 0,

    [Display(Name = "در انتظار تایید قیمت پیشنهادی")]
    WaitingForConfirmOfferedPrice = 10,

    [Display(Name = "تعیین قیمت نهایی شده")]
    PriceFinalized = 20,

    [Display(Name = "در حال جستجو")]
    Searching = 30,

    [Display(Name = "در حال انجام")]
    InProgress = 50,

    [Display(Name = "بسته شده")]
    Closed = 60,
}