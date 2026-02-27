using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class GatewayService(IConfiguration config, ILogger<GatewayService> logger, IMapper mapper,
        HttpClient httpClient) : IGatewayService
    {

        private readonly IConfiguration _config = config;
        private readonly ILogger<GatewayService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> RegisterAsync(RegisterDTO request)
        {
            try
            {
                var baseUrl = _config["RegistrationUrl"];

                if (string.IsNullOrEmpty(baseUrl))
                    throw new InvalidOperationException("RegistrationUrl is not configured.");

                var url = $"{baseUrl}/register/register";
                _logger.LogInformation("Sending registration request for {Email}", request.Email);

                var response = await _httpClient.PostAsJsonAsync(url, request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Registration failed for {Email}. Status: {StatusCode}. Response: {Response}",
                        request.Email,
                        response.StatusCode,
                        responseContent);

                    throw new ApplicationException(responseContent);
                }

                _logger.LogInformation("User registered successfully: {Email}", request.Email);
                var res = $"User registered successfully request.";
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user {Email}", request.Email);
                throw;
            }

        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO request)
        {
            try
            {
                var baseUrl = _config["UserbaseUrl"];
                var url = $"{baseUrl}/login";
                _logger.LogInformation("Sending login request for {Email}", request.Email);
                var response = await _httpClient.PostAsJsonAsync(url, request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login failed for {Email}. Status: {StatusCode}. Response: {Response}",
                        request.Email,
                        response.StatusCode,
                        responseContent);

                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<User>(responseContent,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null || !BCrypt.Net.BCrypt.Verify(request.Password, result.PasswordHash))
                {
                    _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                return GenerateToken(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user {Email}", request.Email);
                throw;
            }
        }

        public async Task<UserDTO> GetMyDetailsAsync(string token)
        {
            try
            {
                var baseUrl = _config["UserbaseUrl"];

                if (string.IsNullOrEmpty(baseUrl)) throw new InvalidOperationException("UserbaseUrl is not configured.");

                var url = $"{baseUrl}/myDetails";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);
                _logger.LogInformation("Forwarding MyDetails request to User service");
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MyDetails failed. Status: {StatusCode}. Response: {Response}",
                        response.StatusCode,
                        responseContent);

                    throw new UnauthorizedAccessException("Unauthorized request.");
                }

                var result = await response.Content.ReadFromJsonAsync<UserDTO>();

                return result ?? throw new ApplicationException("Failed to deserialize UserDTO.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching MyDetails");
                throw;
            }
        }

        private AuthResponseDTO GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            AuthResponseDTO accessToken = new() { AccessToken = jwt };

            return accessToken;
        }
    }
}
