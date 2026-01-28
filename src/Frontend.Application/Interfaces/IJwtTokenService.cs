namespace Frontend.Application.Interfaces;

public interface IJwtTokenService
{
    Task<string> GetTokenAsync();
}
