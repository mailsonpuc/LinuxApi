using System.Text.Json;
using DistroFront.DTOs;
using DistroFront.Http;

namespace DistroFront.Services;

public interface IAuthService
{
    event Action? AuthStateChanged;
    Task<UserTokenDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<string> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();
}

public sealed class AuthService : IAuthService
{
    private const string TokenKey = "distro.auth.token";
    private const string ExpirationKey = "distro.auth.expiration";
    private readonly ApiClient _apiClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(ApiClient apiClient, ILocalStorageService localStorage)
    {
        _apiClient = apiClient;
        _localStorage = localStorage;
    }

    public event Action? AuthStateChanged;

    public async Task<UserTokenDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var token = await _apiClient.PostAsync<LoginRequestDto, UserTokenDto>("api/Token/login", request, cancellationToken)
            ?? throw new ApiException(System.Net.HttpStatusCode.Unauthorized, "Login sem token retornado pela API.");

        if (string.IsNullOrWhiteSpace(token.Token))
        {
            throw new ApiException(System.Net.HttpStatusCode.Unauthorized, "Login sem token retornado pela API.");
        }

        await _localStorage.SetItemAsync(TokenKey, token.Token);
        await _localStorage.SetItemAsync(ExpirationKey, token.Expiration.ToString("O"));
        AuthStateChanged?.Invoke();

        return token;
    }

    public async Task<string> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostForTextAsync("api/Token/register", request, cancellationToken);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(ExpirationKey);
        AuthStateChanged?.Invoke();
    }

    public async Task<string?> GetTokenAsync()
    {
        var token = await _localStorage.GetItemAsync(TokenKey);
        var expirationText = await _localStorage.GetItemAsync(ExpirationKey);

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(expirationText))
        {
            return null;
        }

        if (!DateTime.TryParse(expirationText, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiration))
        {
            await LogoutAsync();
            return null;
        }

        if (expiration <= DateTime.UtcNow)
        {
            await LogoutAsync();
            return null;
        }

        return token;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        return !string.IsNullOrWhiteSpace(await GetTokenAsync());
    }
}
