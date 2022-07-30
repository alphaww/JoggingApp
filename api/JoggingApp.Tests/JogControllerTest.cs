using JoggingApp.Core.Crypto;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Users;
using JoggingApp.Jogs;
using JoggingApp.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace JoggingApp.Tests
{
    public class JogControllerTest
    {
        private readonly JogController _controller;
        private readonly IJogStorage _jogStorage;
        private readonly IUserStorage _userStorage;
        private readonly IHashService _hashService;

        private string user1Email = "testuser1@test.com";
        private string user2Email = "testuser2@test.com";

        public JogControllerTest(JogController controller, IJogStorage jogStorage, IUserStorage userStorage, IHashService hashService)
        {
            _controller = controller;
            _jogStorage = jogStorage;
            _userStorage = userStorage;
            _hashService = hashService;
        }

        public static readonly object[][] SearchJogsTestParams =
        {
            new object[] { new DateTime(2021, 1, 1), new DateTime(2021, 10, 18),  0},
            new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 1, 1),  1},
            new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 2, 20), 2},
            new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 20), 3},
            new object[] { null, null, 4},
        };

        [Theory, MemberData(nameof(SearchJogsTestParams))]
        public async void SearchAsync_Should_Return_Jogs_Filtered_By_Date_If_Filter_Params_Provided(DateTime? from, DateTime? to, int expectedCount)
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                          new Claim(ClaimTypes.Email, user1.Email),
                                          new Claim("id", user1.Id.ToString())
                                   }, "TestAuthentication"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var result = await _controller.SearchAsync(from, to, CancellationToken.None);

            Assert.True(result.Count() == expectedCount);
        }
    }
}
