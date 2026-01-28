using System.Net.Http.Json;
using Frontend.Application.Configuration;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Frontend.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private string? _token;
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _settings;

    public JwtTokenService(HttpClient httpClient, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_token))
            return _token;

        var request = new { code = _settings.AuthCode };
        var response = await _httpClient.PostAsJsonAsync(
            $"{_settings.BaseUrl}/api/auth/token", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        
        if (result == null || string.IsNullOrEmpty(result.Token))
            throw new InvalidOperationException("Failed to obtain JWT token");

        _token = result.Token;
        return _token;
    }
}
