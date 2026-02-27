using Application.DTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace UnitTests.Integration
{

    public class GatewayIntegrationTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Register_Login_And_GetMyDetails_Should_Work_EndToEnd()
        {
            var registerRequest = new RegisterDTO
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@ufh.ac.za",
                Password = "Password123"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/gateWay/register", registerRequest);

            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            var registerResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponseDTO>();

            Assert.NotNull(registerResult);
            Assert.False(string.IsNullOrEmpty(registerResult!.AccessToken));

            var loginRequest = new LoginDTO
            {
                Email = "test@ufh.ac.za",
                Password = "Password123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/gateWay/login", loginRequest);

            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDTO>();

            Assert.NotNull(loginResult);
            Assert.False(string.IsNullOrEmpty(loginResult!.AccessToken));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);

            var userResponse = await _client.GetAsync("/api/users/myDetails");

            Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);

            var userResult = await userResponse.Content.ReadFromJsonAsync<UserDTO>();

            Assert.NotNull(userResult);
            Assert.Equal("test@ufh.ac.za", userResult!.Email);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_Should_Fail()
        {
            var request = new RegisterDTO
            {
                Email = "duplicate@test.com",
                Password = "Password123"
            };

            await _client.PostAsJsonAsync("/api/gateWay/register", request);

            var secondResponse = await _client.PostAsJsonAsync("/api/gateWay/register", request);

            Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        }

        [Fact]
        public async Task Login_WithWrongPassword_Should_ReturnUnauthorized()
        {
            var registerRequest = new RegisterDTO
            {
                Email = "wrongpass@test.com",
                Password = "Password123"
            };

            await _client.PostAsJsonAsync("/api/gateWay/register", registerRequest);

            var loginRequest = new LoginDTO
            {
                Email = "wrongpass@test.com",
                Password = "WrongPassword"
            };

            var response = await _client.PostAsJsonAsync("/api/gateWay/login", loginRequest);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task MyDetails_WithoutToken_Should_ReturnUnauthorized()
        {
            var response = await _client.GetAsync("/api/users/myDetails");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}