namespace DistroFront.Models;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public PaginationMetadata Metadata { get; init; } = new();
}
