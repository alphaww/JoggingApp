using FluentValidation.Results;
using JoggingApp.Core;
using JoggingApp.Users;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace JoggingApp.Tests
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
      
        public UserControllerTest(UserController controller)
        {
            _controller = controller;
        }

        [Fact]
        public async void Authenticate_Should_Return_Unauthorized_If_User_Not_Registered()
        {
            var request = new UserAuthRequest
            {
                Email = "somedummy@gmail.com",
                Password = "1234"
            };
            // Act
            var result = await _controller.Atuhenticate(request, CancellationToken.None);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Authenticate_Should_Return_Ok_If_User_Is_Registered()
        {
            var registerRequest = new UserRegisterRequest
            {
                Email = "somedummy@gmail.com",
                Password = "1234"
            };

            await _controller.Register(registerRequest, CancellationToken.None);

            var authRequest = new UserAuthRequest
            {
                Email = "somedummy@gmail.com",
                Password = "1234"
            };
            // Act
            var result = await _controller.Atuhenticate(authRequest, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Register_Should_Return_Conflict_If_Registering_User_That_Exists()
        {
            var registerRequest = new UserRegisterRequest
            {
                Email = "somedummy@gmail.com",
                Password = "1234"
            };

            await _controller.Register(registerRequest, CancellationToken.None);

            // Act
            var result = await _controller.Register(registerRequest, CancellationToken.None);

            // Assert
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
            var result = await _controller.Register(registerRequest, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            var errors = badRequestResult.Value as Dictionary<string, string[]>;

            // 1) email is not a valid email, 2) email is empty
            Assert.True(errors[nameof(UserRegisterRequest.Email)].Count() == 2);

            // 1) password is empty
            Assert.True(errors[nameof(UserRegisterRequest.Password)].Count() == 1);
        }
    }
}
