using System.Net.Http.Json;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Frontend.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private string? _token;
    private DateTime _tokenExpiry = DateTime.MinValue;
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _settings;

    public JwtTokenService(HttpClient httpClient, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_token) && DateTime.UtcNow < _tokenExpiry)
            return _token;

        var request = new { apiToken = _settings.AuthCode };
        var response = await _httpClient.PostAsJsonAsync(
            $"{_settings.BaseUrl}/api/auth/token", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        
        if (result == null || string.IsNullOrEmpty(result.Token))
            throw new InvalidOperationException("Failed to obtain JWT token");

        _token = result.Token;
        _tokenExpiry = result.ExpiresAt.AddMinutes(-5);
        
        return _token;
    }
}
