using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Configuration;
using Frontend.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure API Settings
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

        // Register HttpClient services
        services.AddHttpClient<IClientesApiService, ClientesApiService>();
        services.AddHttpClient<IVehiculosApiService, VehiculosApiService>();
        services.AddHttpClient<IExtrasApiService, ExtrasApiService>();
        services.AddHttpClient<ICategoriasApiService, CategoriasApiService>();

        // Register JWT Service as Singleton
        services.AddSingleton<IJwtTokenService>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiSettings>>();
            return new JwtTokenService(httpClient, settings);
        });

        return services;
    }
}
