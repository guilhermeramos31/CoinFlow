namespace CoinFlow.Models.UserEntity.Dto;

using Tokens;

public class LoginResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public UserResponse User { get; set; }
    public Tokens Tokens { get; set; }
}
