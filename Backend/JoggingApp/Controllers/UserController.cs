using JoggingApp.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JoggingApp.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserStorage _userStorage;
        private readonly IHashService _hashService;
        private readonly ITokenWriter _tokenWriter;
        public UserController(IUserStorage userStorage, IHashService hashService, ITokenWriter tokenWriter)
        {
            _userStorage = userStorage ?? throw new ArgumentNullException(nameof(userStorage));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _tokenWriter = tokenWriter ?? throw new ArgumentNullException(nameof(tokenWriter));
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            var user = Core.User.Create(request.Email, request.Password, _hashService);
            var registrationResult = await _userStorage.SaveAsync(user);
            if (registrationResult == UserRegistrationResult.Success)
            {
                return Ok();
            }
            return Conflict($"User with email {request.Email} already exists.");
        }

        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Atuhenticate(UserAuthRequest request)
        {
            var hashedPassword = _hashService.Hash(request.Password);
            var user = await _userStorage.FindByEmailAndPasswordAsync(request.Email, hashedPassword);
            if (user is not null)
            {
                var jwtToken = _tokenWriter.Write(user);
                return Ok(new UserAuthResponse(user.Email, jwtToken));
            }
            return Unauthorized();
        }
    }
}
