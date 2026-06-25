using DistroFront.DTOs;
using DistroFront.Http;
using DistroFront.Models;

namespace DistroFront.Services;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<CategoryDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateAsync(CategoryDto category, CancellationToken cancellationToken = default);
    Task<CategoryDto> UpdateAsync(CategoryDto category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public sealed class CategoryService : ICategoryService
{
    private readonly ApiClient _apiClient;

    public CategoryService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetAsync<List<CategoryDto>>("api/Category", cancellationToken) ?? [];
    }

    public async Task<PagedResult<CategoryDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetPagedAsync<CategoryDto>($"api/Category/paginacao?pageNumber={pageNumber}&pageSize={pageSize}", cancellationToken);
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetAsync<CategoryDto>($"api/Category/{id}", cancellationToken);
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto category, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync<CategoryDto, CategoryDto>("api/Category", category, cancellationToken)
            ?? throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Categoria nao retornada pela API.");
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto category, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PutAsync<CategoryDto, CategoryDto>($"api/Category/{category.CategoryId}", category, cancellationToken)
            ?? throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Categoria nao retornada pela API.");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _apiClient.DeleteAsync($"api/Category/{id}", cancellationToken);
    }
}
