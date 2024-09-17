using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Data.Models;
using Shahrah.Framework.Exceptions;
using Shahrah.Framework.Extensions;
using Shahrah.Framework.Resources;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.People.Commands.ChangeMobileNumber;
using Shahrah.Transporter.Application.People.Commands.CloseAccount;
using Shahrah.Transporter.Application.People.Queries.GetIsSubscribed;
using Shahrah.Transporter.Application.People.Queries.GetPerson;
using Shahrah.Transporter.Application.People.Queries.InquiryExistenceMobileNumber;
using Shahrah.Transporter.Application.Transporters.Queries.GetTransporter;
using Shahrah.Transporter.Domain.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Api.Controllers;

public class AccountsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityServerService _identityServerService;

    public AccountsController(IMediator mediator, ICurrentUserService currentUserService, IIdentityServerService identityServerService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _identityServerService = identityServerService;
    }

    [HttpPost("send-verification-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendVerificationToken([FromBody] OtpCodeModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _identityServerService.SendOtpToLogin(model.MobileNumber));
    }

    [HttpPost("verify-token")]
    [ProducesResponseType(typeof(VerificationResponseModel), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyToken([FromBody] CodeValidationModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _identityServerService.VerifyOtp(model.MobileNumber, model.Code));
    }

    [HttpGet("user-info")]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var personInfo = await _mediator.Send(new GetPersonQuery(_currentUserService.Id));
        var transporter = await _mediator.Send(new GetTransporterQuery(_currentUserService.Id));
        var isSubscribed = personInfo != null && await _mediator.Send(new GetIsSubscribedQuery(_currentUserService.Id));
        var personStatus = personInfo?.Status ?? PersonStatus.Active;

        return Ok(new UserModel
        {
            Username = _currentUserService.MobileNumber,
            RegistrationIsComplete = personInfo != null,
            FirstName = personInfo?.FirstName,
            LastName = personInfo?.LastName,
            BirthDate = personInfo?.BirthDate,
            NationalCode = personInfo?.NationalCode,
            TransporterName = transporter?.Name,
            TransporterNationalId = transporter?.NationalId,
            IsSubscribed = isSubscribed,
            Status = personStatus,
            StatusTitle = personStatus.GetDisplayName(),
            IsAgentWaitingForConfirmation = personInfo?.AgentRegistrationStatus == AgentRegistrationStatus.Pending
        });
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<object> RefreshToken(string refreshToken)
    {
        var result = await _identityServerService.RefreshToken(refreshToken);
        if (result == null)
            return Forbid();

        return Ok(result);
    }

    [HttpPost("SendOtpToChangeMobilenumber/{newMobileNmber}")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendOtpToChangeMobilenumber(string newMobileNmber)
    {
        var mobileAlreadyIsExist = await _mediator.Send(new InquiryExistenceMobileNumberQuery(newMobileNmber));
        if (mobileAlreadyIsExist)
            throw new DomainException(ErrorMessageResource.MobileNumberIsNotValidForChange);

        return Ok(await _identityServerService.SendOtpToChangeMobileNumber(_currentUserService.MobileNumber, newMobileNmber));
    }

    [HttpPost("ChangeMobileNumber")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeMobileNumber([FromBody] ChangeMobileNumberModel changeMobileNumber)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _mediator.Send(new ChangeMobileNumberCommand(_currentUserService.Id, changeMobileNumber.NewMobileNumber, changeMobileNumber.Otp));
        return Ok();
    }

    [HttpPost("Close")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> CloseAccount()
    {
        await _mediator.Send(new CloseAccountCommand(_currentUserService.Id));
        return Ok();
    }
}