using System.Net.Http.Headers;
using System.Net.Http.Json;
using Frontend.Application.Configuration;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Frontend.Application.Services;

public class VehiculosApiService : IVehiculosApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJwtTokenService _jwtService;
    private readonly ApiSettings _settings;

    public VehiculosApiService(HttpClient httpClient, IJwtTokenService jwtService, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _jwtService = jwtService;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<VehiculoDto>> ListarAsync()
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/vehiculos");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<VehiculoDto>>();
        return result ?? Enumerable.Empty<VehiculoDto>();
    }

    public async Task<VehiculoDto?> ObtenerAsync(int id)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/vehiculos/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VehiculoDto>();
    }

    public async Task<int> CrearAsync(VehiculoDto dto)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/api/vehiculos", dto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<VehiculoDto>();
        return result?.IdVehiculo ?? 0;
    }

    public async Task<bool> ActualizarAsync(int id, VehiculoDto dto)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.PutAsJsonAsync($"{_settings.BaseUrl}/api/vehiculos/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.DeleteAsync($"{_settings.BaseUrl}/api/vehiculos/{id}");
        return response.IsSuccessStatusCode;
    }

    private async Task EnsureJwtTokenAsync()
    {
        var token = await _jwtService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
