using JoggingApp.Core.Crypto;
using JoggingApp.Core.Users;
using JoggingApp.Users;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace JoggingApp.Tests
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly IUserStorage _userStorage;
        private readonly IHashService _hashService;
      
        public UserControllerTest(UserController controller, IUserStorage userStorage, IHashService hashService)
        {
            _controller = controller;
            _userStorage = userStorage;
            _hashService = hashService;
        }

        [Fact]
        public async void Authenticate_Should_Return_Unauthorized_If_User_Not_Registered()
        {
            var request = new UserAuthRequest
            {
                Email = "somedummy1@gmail.com",
                Password = "1234"
            };
            // Act
            var result = await _controller.LogInAsync(request, CancellationToken.None);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Authenticate_Should_Return_Unauthorized_If_User_Is_Registered_But_Not_Confirmed()
        {
            var email = "somedummy2@gmail.com";
            var password = "aaba";

            var registerRequest = new UserRegisterRequest
            {
                Email = email,
                Password = password
            };

            await _controller.RegisterAsync(registerRequest, CancellationToken.None);

            var authRequest = new UserAuthRequest
            {
                Email = email,
                Password = password
            };

            var result = await _controller.LogInAsync(authRequest, CancellationToken.None);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Authenticate_Should_Return_Ok_If_User_Is_Registered_And_Confirmed()
        {
            var email = "somedummy3@gmail.com";
            var password = "?Pass123.3";
            var hashedPassword = _hashService.Hash(password);

            var registerRequest = new UserRegisterRequest
            {
                Email = email,
                Password = password
            };

            await _controller.RegisterAsync(registerRequest, CancellationToken.None);

            var registeredUser = await _userStorage.FindByEmailAsync(email, CancellationToken.None);

            Assert.Single(registeredUser.ActivationTokens);

            var activationToken = registeredUser.ActivationTokens.Single();

            await _controller.ConfirmAsync(activationToken.Id, CancellationToken.None);

            var registeredUser2 = await _userStorage.FindByEmailAsync(email, CancellationToken.None);

            var authRequest = new UserAuthRequest
            {
                Email = email,
                Password = password
            };

            var result = await _controller.LogInAsync(authRequest, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Register_Should_Create_A_Valid_New_Inactive_User_And_Return_Ok()
        {
            var email = "somedummy4@gmail.com";
            var password = "?Pass123.4";
            var hashedPassword = _hashService.Hash(password);

            var registerRequest = new UserRegisterRequest
            {
                Email = email,
                Password = password
            };

            var result = await _controller.RegisterAsync(registerRequest, CancellationToken.None);

            var registeredUser = await _userStorage.FindByEmailAsync(email, CancellationToken.None);

            Assert.Equal(email, registeredUser.Email);
            Assert.Equal(hashedPassword, registeredUser.Password);
            Assert.Equal(UserState.Inactive, registeredUser.State);
            Assert.Single(registeredUser.ActivationTokens);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Register_Should_Return_Conflict_If_Registering_User_That_Already_Exists()
        {
            var registerRequest = new UserRegisterRequest
            {
                Email = "somedummy5@gmail.com",
                Password = "?Pass123.5"
            };

            await _controller.RegisterAsync(registerRequest, CancellationToken.None);

            var result = await _controller.RegisterAsync(registerRequest, CancellationToken.None);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async void Register_Should_Return_BadRequest_And_Validation_Errors_If_Request_Invalid()
        {
            var registerRequest = new UserRegisterRequest
            {
                Email = "",
                Password = ""
            };

            // Act
            var result = await _controller.RegisterAsync(registerRequest, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            var errors = badRequestResult.Value as Dictionary<string, string[]>;

            // 1) email is not a valid email, 2) email is empty
            Assert.True(errors[nameof(UserRegisterRequest.Email)].Count() == 2);

            // 1) password is empty and all the password strength rules
            Assert.True(errors[nameof(UserRegisterRequest.Password)].Count() == 6);
        }
    }
}
