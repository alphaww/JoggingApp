using JoggingApp.Core.Crypto;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;
using JoggingApp.Jogs;
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

        private void SetUpPrincipal(User user)
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                          new Claim(ClaimTypes.Email, user.Email),
                                          new Claim("id", user.Id.ToString())
                                   }, "TestAuthentication"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
        }

        public static readonly object[][] SearchJogsTestParams =
        {
            new object[] { new DateTime(2021, 1, 1), new DateTime(2021, 10, 18),  0},
            new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 1, 1),  1},
            new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 2, 20), 2},
            new object[] { new DateTime(2022, 1, 1), new DateTime(2022, 3, 20), 3},
            new object[] { null, null, 4}
        };

        [Theory, MemberData(nameof(SearchJogsTestParams))]
        public async void SearchAsync_Should_Return_Jogs_Filtered_By_Date_If_Filter_Params_Provided(DateTime? from, DateTime? to, int expectedCount)
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);

            SetUpPrincipal(user1);

            var result = await _controller.SearchAsync(from, to, CancellationToken.None);

            Assert.True(result.Count() == expectedCount);
        }

        [Fact]
        public async void GetAsync_Should_Return_Jog_Entry_For_User_That_Created_It()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            var user1Jogs = await _jogStorage.SearchAsync(user1.Id, null, null);

            SetUpPrincipal(user1);

            var result = await _controller.GetAsync(user1Jogs.First().Id, CancellationToken.None);

            Assert.IsType<JogDto>(result);
            Assert.True(result is not null);
        }

        [Fact]
        public async void GetAsync_Should_Not_Return_Jog_Entry_For_User_That_Did_Not_Create_It()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            var user2 = await _userStorage.FindByEmailAsync(user2Email, CancellationToken.None);
            var user1Jogs = await _jogStorage.SearchAsync(user1.Id, null, null);

            SetUpPrincipal(user2);

            var result = await _controller.GetAsync(user1Jogs.First().Id, CancellationToken.None);

            Assert.True(result is null);
        }

        [Fact]
        public async void InsertAsync_Should_Return_BadRequest_And_Validation_Errors_If_Request_Invalid()
        {
            var insertJogRequest = new JogInsertRequest
            {
                Coordinates = new Coordinates
                {
                    Latitude = -11111,
                    Longitude = -22222
                },
                Distance = -5,
                Time = new RunningTimeDto
                {
                    Hours = -1,
                    Minutes = -1,
                    Seconds = -1
                }
            };

            // Act
            var result = await _controller.InsertAsync(insertJogRequest, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            var errors = badRequestResult.Value as Dictionary<string, string[]>;

            // 1) time must be in proper range
            Assert.True(errors[nameof(JogInsertRequest.Time)].Count() == 1);

            // 2) distance must be in proper range
            Assert.True(errors[nameof(JogInsertRequest.Distance)].Count() == 1);

            // 3) coordinates must be valid
            Assert.True(errors[nameof(JogInsertRequest.Coordinates)].Count() == 2);
        }

        [Fact]
        public async void InsertAsync_Should_Return_Ok_And_Insert_New_Jog_Entry_For_User1_Without_WeatherInfo_If_WeatherService_Throws()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            SetUpPrincipal(user1);

            var insertJogRequest = new JogInsertRequest
            {
                Coordinates = new Coordinates
                {
                    Latitude = 0,
                    Longitude = 0
                },
                Distance = 1000,
                Time = new RunningTimeDto
                {
                    Hours = 3,
                    Minutes = 0,
                    Seconds = 0
                }
            };

            var result = await _controller.InsertAsync(insertJogRequest, CancellationToken.None);

            //SearchAsync should return all the jog entries in the DB and order it descending.
            var insertedJog = (await _jogStorage.SearchAsync(null, null, null)).OrderByDescending(x => x.Date).Take(1).SingleOrDefault();

            Assert.IsType<OkResult>(result);
            Assert.True(insertedJog is not null);
            Assert.True(insertedJog.JogLocation is null);
            Assert.True(insertedJog.UserId == user1.Id);
        }

        [Fact]
        public async void InsertAsync_Should_Return_Ok_And_Insert_New_Jog_For_User2_With_WeatherInfo_If_WeatherService_Succeeds()
        {
            var user2 = await _userStorage.FindByEmailAsync(user2Email, CancellationToken.None);
            SetUpPrincipal(user2);

            var insertJogRequest = new JogInsertRequest
            {
                Coordinates = new Coordinates
                {
                    Latitude = 45.815399,
                    Longitude = 15.966568
                },
                Distance = 1000,
                Time = new RunningTimeDto
                {
                    Hours = 3,
                    Minutes = 0,
                    Seconds = 0
                }
            };

            var result = await _controller.InsertAsync(insertJogRequest, CancellationToken.None);

            //SearchAsync should return all the jog entries in the DB and order it descending.
            var insertedJog = (await _jogStorage.SearchAsync(null, null, null)).OrderByDescending(x => x.Date).Take(1).SingleOrDefault();

            Assert.IsType<OkResult>(result);
            Assert.True(insertedJog is not null);
            Assert.True(insertedJog.JogLocation is not null);
            Assert.True(insertedJog.UserId == user2.Id);
        }

        [Fact]
        public async void UpdateAsync_Should_Return_BadRequest_And_Validation_Errors_If_Request_Invalid()
        {
            var updateJogRequest = new JogUpdateRequest
            {
                Distance = -5,
                Time = new RunningTimeDto
                {
                    Hours = -1,
                    Minutes = -1,
                    Seconds = -1
                }
            };

            var jogToUpdate = (await _jogStorage.SearchAsync(null, null, null)).First();

            var result = await _controller.UpdateAsync(jogToUpdate.Id, updateJogRequest, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            var errors = badRequestResult.Value as Dictionary<string, string[]>;

            // 1) time must be in proper range
            Assert.True(errors[nameof(JogUpdateRequest.Time)].Count() == 1);

            // 2) distance must be in proper range
            Assert.True(errors[nameof(JogUpdateRequest.Distance)].Count() == 1);
        }

        [Fact]
        public async void UpdateAsync_Should_Return_NotFound_If_Updating_Non_Existing_Jog()
        {
            var updateJogRequest = new JogUpdateRequest
            {
                Distance = 10000,
                Time = new RunningTimeDto
                {
                    Hours = 2,
                    Minutes = 0,
                    Seconds = 0
                }
            };

            var result = await _controller.UpdateAsync(Guid.NewGuid(), updateJogRequest, CancellationToken.None);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateAsync_Should_Return_BadRequest_If_Updating_Jog_That_You_Dont_Own()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            var user2 = await _userStorage.FindByEmailAsync(user2Email, CancellationToken.None);
            SetUpPrincipal(user2);

            var updateJogRequest = new JogUpdateRequest
            {
                Distance = 10000,
                Time = new RunningTimeDto
                {
                    Hours = 2,
                    Minutes = 0,
                    Seconds = 0
                }
            };

            var jogToUpdate = (await _jogStorage.SearchAsync(user1.Id, null, null)).First();

            var result = await _controller.UpdateAsync(jogToUpdate.Id, updateJogRequest, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdateAsync_Should_Return_Ok_And_Do_Correct_Update_If_Updating_Jog_That_You_Own()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            SetUpPrincipal(user1);

            var updateJogRequest = new JogUpdateRequest
            {
                Distance = 99002,
                Time = new RunningTimeDto
                {
                    Hours = 12,
                    Minutes = 24,
                    Seconds = 33
                }
            };

            var jogToUpdate = (await _jogStorage.SearchAsync(user1.Id, null, null)).First();

            var result = await _controller.UpdateAsync(jogToUpdate.Id, updateJogRequest, CancellationToken.None);

            var updatedJog = await _jogStorage.GetByJogIdAsync(jogToUpdate.Id);

            Assert.IsType<OkResult>(result);
            Assert.True(updatedJog.Distance == updateJogRequest.Distance);
            Assert.True(updatedJog.Time == updateJogRequest.Time.ToTimeSpan());
            Assert.True(updatedJog.Id == jogToUpdate.Id);

        }

        [Fact]
        public async void DeleteAsync_Should_Return_NotFound_If_Deleting_Non_Existing_Jog()
        {
            var result = await _controller.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteAsync_Should_Return_BadRequest_If_Deleting_Jog_That_You_Dont_Own()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            var user2 = await _userStorage.FindByEmailAsync(user2Email, CancellationToken.None);
            SetUpPrincipal(user2);

            var jogToUpdate = (await _jogStorage.SearchAsync(user1.Id, null, null)).First();

            var result = await _controller.DeleteAsync(jogToUpdate.Id, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteAsync_Should_Return_Ok_And_Do_Correct_Delete_If_Deleting_Jog_That_You_Own()
        {
            var user1 = await _userStorage.FindByEmailAsync(user1Email, CancellationToken.None);
            SetUpPrincipal(user1);

            var jogToDelete = (await _jogStorage.SearchAsync(user1.Id, null, null)).First();

            var result = await _controller.DeleteAsync(jogToDelete.Id, CancellationToken.None);

            var deletedJog = await _jogStorage.GetByJogIdAsync(jogToDelete.Id);

            Assert.IsType<OkResult>(result);
            Assert.True(deletedJog is null);

        }
    }
}
