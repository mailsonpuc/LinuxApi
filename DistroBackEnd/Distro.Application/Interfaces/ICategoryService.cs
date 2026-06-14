using Distro.Application.DTOs;
using Distro.Domain.Pagination;

namespace Distro.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<PagedList<CategoryDTO>> GetCategoriesPaged(int pageNumber = 1, int pageSize = 10);
        Task<CategoryDTO> GetCategoryById(Guid? id);
        Task<CategoryDTO> CreateCategory(CategoryDTO categoryDto);
        Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDto);
        Task<bool> DeleteCategory(Guid? id);
    }
}