using System.Net.Http.Headers;
using DistroFront.Services;

namespace DistroFront.Http;

public sealed class AuthHeaderHandler : DelegatingHandler
{
    private const string TokenKey = "distro.auth.token";
    private const string ExpirationKey = "distro.auth.expiration";
    private readonly ILocalStorageService _localStorage;

    public AuthHeaderHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetValidTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string?> GetValidTokenAsync()
    {
        var token = await _localStorage.GetItemAsync(TokenKey);
        var expirationText = await _localStorage.GetItemAsync(ExpirationKey);

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(expirationText))
        {
            return null;
        }

        if (!DateTime.TryParse(expirationText, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiration))
        {
            return null;
        }

        return expiration > DateTime.UtcNow ? token : null;
    }
}
