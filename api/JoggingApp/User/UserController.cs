using FluentValidation;
using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Email;
using JoggingApp.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Users
{
    [Route("user")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserRegisterRequest> _userRegisterRequestValidator;
        private readonly IValidator<UserAuthRequest> _userAuthRequestValidator;
        private readonly IUserStorage _userStorage;
        private readonly IHashService _hashService;
        private readonly ITokenWriter _tokenWriter;
        private readonly IEmailSender _emailSender;
        private readonly UserRegisteredEmailTemplateRenderer _userRegisteredEmailTemplateRenderer;
        public UserController(
            IValidator<UserRegisterRequest> userRegisterRequestValidator,
            IValidator<UserAuthRequest> userAuthRequestValidator,
            IUserStorage userStorage, 
            IHashService hashService,
            ITokenWriter tokenWriter,
            IEmailSender emailSender,
            UserRegisteredEmailTemplateRenderer userRegisteredEmailTemplateRenderer)
        {
            _userRegisterRequestValidator = userRegisterRequestValidator ?? throw new ArgumentNullException(nameof(userRegisterRequestValidator));
            _userAuthRequestValidator = userAuthRequestValidator ?? throw new ArgumentNullException(nameof(userAuthRequestValidator));
            _userStorage = userStorage ?? throw new ArgumentNullException(nameof(userStorage));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _tokenWriter = tokenWriter ?? throw new ArgumentNullException(nameof(tokenWriter));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _userRegisteredEmailTemplateRenderer = userRegisteredEmailTemplateRenderer;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request, CancellationToken cancellation)
        {
            var validationResult = await _userRegisterRequestValidator.ValidateAsync(request, cancellation);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var existingUser = await _userStorage.FindByEmailAsync(request.Email, cancellation);
            if (existingUser is not null)
            {
                return Conflict($"User with email {request.Email} already exists.");
            }
            var (user, activationToken) = Core.Users.User.Create(request.Email, request.Password, _hashService);
            await _userStorage.InsertAsync(user, cancellation);
            var emailTemplate = await _userRegisteredEmailTemplateRenderer.RenderForUserActivationToken(activationToken);
            await _emailSender.SendAsync(new MailMessage(request.Email, "Confirm account", emailTemplate));
            return Ok();      
        }

        [HttpPost]
        [Route("log-in")]
        public async Task<IActionResult> Atuhenticate(UserAuthRequest request, CancellationToken cancellation)
        {
            var validationResult = await _userAuthRequestValidator.ValidateAsync(request, cancellation);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var hashedPassword = _hashService.Hash(request.Password);
            var user = await _userStorage.FindActiveByEmailAndPasswordAsync(request.Email, hashedPassword, cancellation);
            if (user is not null)
            {
                var jwtToken = _tokenWriter.Write(user);
                return Ok(new UserAuthResponse(user.Email, jwtToken));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("{activationTokenId:guid}/confirm")]
        public async Task<IActionResult> Confirm(Guid activationTokenId, CancellationToken cancellation)
        {
            var user = await _userStorage.FindByActivationTokenId(activationTokenId, cancellation);
            if (user is null)
            {
                return BadRequest($"Activation token does not exist. Please register and we will send you one to your email.");
            }
            if (!user.HasValidActivationToken())
            {
                return BadRequest($"Your activation token has expired.");
            }
            user.Activate();
            await _userStorage.ConfirmAsync(user, cancellation);
            return Ok();
        }
    }
}
