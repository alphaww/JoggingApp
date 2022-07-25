using FluentValidation;
using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Users
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserRegisterRequest> _userRegisterRequestValidator;
        private readonly IValidator<UserAuthRequest> _userAuthRequestValidator;
        private readonly IUserStorage _userStorage;
        private readonly IHashService _hashService;
        private readonly ITokenWriter _tokenWriter;
        public UserController(
            IValidator<UserRegisterRequest> userRegisterRequestValidator,
            IValidator<UserAuthRequest> userAuthRequestValidator,
            IUserStorage userStorage, 
            IHashService hashService,
            ITokenWriter tokenWriter)
        {
            _userRegisterRequestValidator = userRegisterRequestValidator ?? throw new ArgumentNullException(nameof(userRegisterRequestValidator));
            _userAuthRequestValidator = userAuthRequestValidator ?? throw new ArgumentNullException(nameof(userAuthRequestValidator));
            _userStorage = userStorage ?? throw new ArgumentNullException(nameof(userStorage));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _tokenWriter = tokenWriter ?? throw new ArgumentNullException(nameof(tokenWriter));
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
            var user = Core.Users.User.Create(request.Email, request.Password, _hashService);
            await _userStorage.SaveAsync(user, cancellation);
            return Ok();      
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Atuhenticate(UserAuthRequest request, CancellationToken cancellation)
        {
            var validationResult = await _userAuthRequestValidator.ValidateAsync(request, cancellation);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var hashedPassword = _hashService.Hash(request.Password);
            var user = await _userStorage.FindByEmailAndPasswordAsync(request.Email, hashedPassword, cancellation);
            if (user is not null)
            {
                var jwtToken = _tokenWriter.Write(user);
                return Ok(new UserAuthResponse(user.Email, jwtToken));
            }
            return Unauthorized();
        }
    }
}
