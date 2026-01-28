namespace Frontend.Application.DTOs;

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
