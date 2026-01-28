using Frontend.Application.Configuration;
using Frontend.Application.Interfaces;
using Frontend.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure API Settings
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

// Register HttpClient services
builder.Services.AddHttpClient<IClientesApiService, ClientesApiService>();
builder.Services.AddHttpClient<IVehiculosApiService, VehiculosApiService>();
builder.Services.AddHttpClient<IExtrasApiService, ExtrasApiService>();
builder.Services.AddHttpClient<ICategoriasApiService, CategoriasApiService>();

// Register JWT Service as Singleton
builder.Services.AddSingleton<IJwtTokenService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiSettings>>();
    return new JwtTokenService(httpClient, settings);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
