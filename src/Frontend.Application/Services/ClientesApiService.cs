using System.Net.Http.Headers;
using System.Net.Http.Json;
using Frontend.Application.Configuration;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Frontend.Application.Services;

public class ClientesApiService : IClientesApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJwtTokenService _jwtService;
    private readonly ApiSettings _settings;

    public ClientesApiService(HttpClient httpClient, IJwtTokenService jwtService, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _jwtService = jwtService;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<ClienteDto>> ListarAsync()
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/clientes");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ClienteDto>>();
        return result ?? Enumerable.Empty<ClienteDto>();
    }

    public async Task<ClienteDto?> ObtenerAsync(int id)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/clientes/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ClienteDto>();
    }

    public async Task<int> CrearAsync(ClienteDto dto)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/api/clientes", dto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ClienteDto>();
        return result?.IdCliente ?? 0;
    }

    public async Task<bool> ActualizarAsync(int id, ClienteDto dto)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.PutAsJsonAsync($"{_settings.BaseUrl}/api/clientes/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.DeleteAsync($"{_settings.BaseUrl}/api/clientes/{id}");
        return response.IsSuccessStatusCode;
    }

    private async Task EnsureJwtTokenAsync()
    {
        var token = await _jwtService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
