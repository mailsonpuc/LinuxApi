using System.Net.Http.Json;
using System.Text.Json;
using DistroFront.Models;

namespace DistroFront.Http;

public sealed class ApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(uri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
    }

    public async Task<PagedResult<T>> GetPagedAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(uri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var items = await response.Content.ReadFromJsonAsync<List<T>>(JsonOptions, cancellationToken) ?? [];
        var metadata = new PaginationMetadata
        {
            TotalCount = items.Count,
            PageSize = items.Count,
            CurrentPage = 1,
            TotalPages = items.Count == 0 ? 0 : 1
        };

        if (response.Headers.TryGetValues("X-Pagination", out var values))
        {
            var header = values.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(header))
            {
                metadata = JsonSerializer.Deserialize<PaginationMetadata>(header, JsonOptions) ?? metadata;
            }
        }

        return new PagedResult<T> { Items = items, Metadata = metadata };
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest payload, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(uri, payload, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
    }

    public async Task<string> PostForTextAsync<TRequest>(string uri, TRequest payload, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(uri, payload, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<TResponse?> PostMultipartAsync<TResponse>(string uri, MultipartFormDataContent content, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsync(uri, content, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest payload, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(uri, payload, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
    }

    public async Task DeleteAsync(string uri, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(uri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(message))
        {
            message = response.ReasonPhrase ?? "Nao foi possivel concluir a requisicao.";
        }

        throw new ApiException(response.StatusCode, message.Trim('"'));
    }
}
