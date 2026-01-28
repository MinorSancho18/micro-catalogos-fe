using System.Net.Http.Headers;
using System.Net.Http.Json;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Frontend.Infrastructure.Services;

public class CategoriasApiService : ICategoriasApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJwtTokenService _jwtService;
    private readonly ApiSettings _settings;

    public CategoriasApiService(HttpClient httpClient, IJwtTokenService jwtService, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _jwtService = jwtService;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<CategoriaVehiculoDto>> ListarAsync()
    {
        await EnsureJwtTokenAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/api/categorias-vehiculo");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<CategoriaVehiculoDto>>();
        return result ?? Enumerable.Empty<CategoriaVehiculoDto>();
    }

    private async Task EnsureJwtTokenAsync()
    {
        var token = await _jwtService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
