using System.Net.Http.Headers;
using System.Net.Http.Json;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Frontend.Infrastructure.Services;

public class ExtrasApiService : IExtrasApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJwtTokenService _jwtService;
    private readonly ApiSettings _settings;

    public ExtrasApiService(HttpClient httpClient, IJwtTokenService jwtService, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _jwtService = jwtService;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<ExtraDto>> ListarAsync()
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/extras");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ExtraDto>>();
        return result ?? Enumerable.Empty<ExtraDto>();
    }

    public async Task<ExtraDto?> ObtenerAsync(int id)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/extras/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExtraDto>();
    }

    public async Task<int> CrearAsync(ExtraDto dto)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/api/extras", dto);
        response.EnsureSuccessStatusCode();
        var id = await response.Content.ReadFromJsonAsync<int>();
        return id;
    }

    public async Task<bool> ActualizarAsync(int id, ExtraDto dto)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.PutAsJsonAsync($"{_settings.BaseUrl}/api/extras/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.DeleteAsync($"{_settings.BaseUrl}/api/extras/{id}");
        return response.IsSuccessStatusCode;
    }

    private async Task EnsureJwtTokenAsync()
    {
        var token = await _jwtService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
