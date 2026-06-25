using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace DistroFront.Services;

public sealed class AuthStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
    private readonly IAuthService _authService;

    public AuthStateProvider(IAuthService authService)
    {
        _authService = authService;
        _authService.AuthStateChanged += NotifyAuthStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(Anonymous);
        }

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private void NotifyAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3)
        {
            return [];
        }

        var payload = parts[1];
        payload += new string('=', (4 - payload.Length % 4) % 4);
        var bytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(bytes) ?? [];

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
    }
}
