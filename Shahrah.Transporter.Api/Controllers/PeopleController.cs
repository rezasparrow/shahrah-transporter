//using MediatR;

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//using Shahrah.Transporter.Api.Authentication;
//using Shahrah.Transporter.Api.Extensions;
//using Shahrah.Transporter.Api.Models;
//using Shahrah.Transporter.Application.Commands;
//using Shahrah.Transporter.Application.DataTransferObjects;
//using Shahrah.Transporter.Application.Queries;
//using Shahrah.Transporter.Domain.Models;

//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;

//namespace Shahrah.Transporter.Api.Controllers
//{
//    public class PeopleController : BaseController
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly IMediator _mediator;
//        private readonly SignInManager<User> _signInManager;
//        private readonly ITokenService _tokenService;

//        public PeopleController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IMediator mediator)
//        {
//            _userManager = userManager;
//            _mediator = mediator;
//            _signInManager = signInManager;
//            _tokenService = tokenService;
//        }

//        [HttpGet("{mobileNumber}/possessions")]
//        [ProducesResponseType(typeof(IEnumerable<UserPossessionDto>), (int)HttpStatusCode.OK)]
//        public async Task<IActionResult> Get(string mobileNumber)
//        {
//            var data = await _mediator.Send(new GetUserPossessionsQuery(mobileNumber));
//            return Ok(data);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
//        public async Task<IActionResult> RegisterRealPerson([FromBody] RegisterPersonModel viewModel)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            await _mediator.Send(new OtpCodeLoginCommand(viewModel.MobileNumber, viewModel.Guid));

//            var realPerson = new Person
//            {
//              //  Email = viewModel.Email,
//                FirstName = viewModel.FirstName,
//                LastName = viewModel.LastName,
//                MobileNumber = viewModel.MobileNumber,
//                NationalCode = viewModel.NationalCode,
//                //PhoneNumber = viewModel.PhoneNumber,
//                //CityId = viewModel.CityId,
//                //Address = viewModel.Address,
//                //PostalCode = viewModel.PostalCode,
//                //NationalCardImageUrl = viewModel.NationalCardImageUrl,
//            };
//            await _mediator.Send(new RegisterPersonCommand(realPerson));

//            var user = new User
//            {
//                UserName = viewModel.MobileNumber,
//                CreateDate = DateTime.Now,
//                SecurityStamp = Guid.NewGuid().ToString(),
//            };
//            var result = await _userManager.CreateAsync(user);
//            if (!result.Succeeded)
//            {
//                ModelState.AddIdentityResultErrors(result);
//                return BadRequest(result.Errors);
//            }
//            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("", ""));
//            await _signInManager.SignInAsync(user, true);


//            var accessToken = _tokenService.GenerateAccessToken(user);
//            var refreshToken = _tokenService.GenerateRefreshToken();

//            return Ok(new UserModel
//            {
//                Username = user.UserName,
//                AccessToken = accessToken,
//                RefreshToken = refreshToken,
//                FirstName=realPerson.FirstName,
//                LastName=realPerson.LastName
//            });
//        }
//    }
//}
