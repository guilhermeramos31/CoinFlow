namespace CoinFlow.Services.Interfaces;

public interface IAuthService
{
    Task<Object> Login(string username, string password);
}
