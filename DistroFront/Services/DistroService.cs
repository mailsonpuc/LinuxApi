using DistroFront.DTOs;
using DistroFront.Http;
using DistroFront.Models;

namespace DistroFront.Services;

public interface IDistroService
{
    Task<IReadOnlyList<DistroDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<DistroDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DistroDto>> SearchAsync(string nome, CancellationToken cancellationToken = default);
    Task<DistroDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DistroDto> CreateAsync(DistroCreateDto distro, CancellationToken cancellationToken = default);
    Task<DistroDto> UpdateAsync(DistroDto distro, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public sealed class DistroService : IDistroService
{
    private const long MaxImageSize = 1024 * 1024;
    private readonly ApiClient _apiClient;

    public DistroService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IReadOnlyList<DistroDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetAsync<List<DistroDto>>("api/Distro", cancellationToken) ?? [];
    }

    public async Task<PagedResult<DistroDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetPagedAsync<DistroDto>($"api/Distro/paginacao?pageNumber={pageNumber}&pageSize={pageSize}", cancellationToken);
    }

    public async Task<IReadOnlyList<DistroDto>> SearchAsync(string nome, CancellationToken cancellationToken = default)
    {
        var encoded = Uri.EscapeDataString(nome);
        return await _apiClient.GetAsync<List<DistroDto>>($"api/Distro/search?nome={encoded}", cancellationToken) ?? [];
    }

    public async Task<DistroDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetAsync<DistroDto>($"api/Distro/{id}", cancellationToken);
    }

    public async Task<DistroDto> CreateAsync(DistroCreateDto distro, CancellationToken cancellationToken = default)
    {
        if (distro.ImageFile is null)
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Selecione uma imagem.");
        }

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(distro.Nome ?? string.Empty), nameof(DistroCreateDto.Nome));
        content.Add(new StringContent(distro.Descricao ?? string.Empty), nameof(DistroCreateDto.Descricao));
        content.Add(new StringContent(distro.Iso ?? string.Empty), nameof(DistroCreateDto.Iso));
        content.Add(new StringContent(distro.CategoryId.ToString()), nameof(DistroCreateDto.CategoryId));

        var fileContent = new StreamContent(distro.ImageFile.OpenReadStream(MaxImageSize));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(distro.ImageFile.ContentType);
        content.Add(fileContent, "ImageFile", distro.ImageFile.Name);

        return await _apiClient.PostMultipartAsync<DistroDto>("api/Distro", content, cancellationToken)
            ?? throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Distro nao retornada pela API.");
    }

    public async Task<DistroDto> UpdateAsync(DistroDto distro, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PutAsync<DistroDto, DistroDto>($"api/Distro/{distro.DistroId}", distro, cancellationToken)
            ?? throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Distro nao retornada pela API.");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _apiClient.DeleteAsync($"api/Distro/{id}", cancellationToken);
    }
}
