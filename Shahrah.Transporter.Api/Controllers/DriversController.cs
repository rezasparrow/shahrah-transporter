using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.ExternalServices;
using Shahrah.Framework.Models;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Application.Drivers.Queries.GetDrivers;
using Shahrah.Transporter.Application.Transporters.Queries.GetTransporter;
using System.Net;
using System.Text.RegularExpressions;

namespace Shahrah.Transporter.Api.Controllers;

public class DriversController(IMediator mediator, ICurrentUserService currentUserService, ISmsService smsService) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ISmsService _smsService = smsService;

    [HttpGet("{nationalCode}")]
    [ProducesResponseType(typeof(Driver), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDriver(string nationalCode)
    {
        var driver = await _mediator.Send(new GetDriversQuery(nationalCode));
        return Ok(driver);
    }

    [HttpPost("{mobileNumber}/SendSms")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendSms(string mobileNumber, [FromServices] AppSettings appSettings)
    {
        if (!Regex.IsMatch(mobileNumber, "^09([0-9]{9})$"))
            return BadRequest(ErrorMessageResource.MobileFormatNotCorrect);

        var transporter = await _mediator.Send(new GetTransporterQuery(_currentUserService.Id));

        await _smsService.Send(mobileNumber, $"توسط باربری {transporter.Name} به سامانه شاهراه دعوت شده اید لطفا جهت ثبت اپلیکیشن شاهراه نسخه راننده را از {appSettings.AppDownloadLink} دانلود فرمایید.");

        return Ok();
    }
}