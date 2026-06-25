using DistroFront.DTOs;
using DistroFront.Http;

namespace DistroFront.Services;

public interface IGenerateService
{
    Task<GenerateResponseDto> GenerateAsync(GenerateRequestDto request, CancellationToken cancellationToken = default);
}

public sealed class GenerateService : IGenerateService
{
    private readonly ApiClient _apiClient;

    public GenerateService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GenerateResponseDto> GenerateAsync(GenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync<GenerateRequestDto, GenerateResponseDto>("api/Generate", request, cancellationToken)
            ?? new GenerateResponseDto();
    }
}
