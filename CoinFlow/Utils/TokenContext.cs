namespace CoinFlow.Utils;

public static class TokenContext
{
    public static string GetToken(this IHttpContextAccessor accessor)
    {
        var token = accessor.Get().Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token)) throw new BadHttpRequestException("Missing token.");
        return token;
    }
}
