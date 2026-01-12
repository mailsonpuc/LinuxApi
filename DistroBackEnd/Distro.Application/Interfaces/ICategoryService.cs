using Distro.Application.DTOs;

namespace Distro.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategoryById(Guid? id);
        Task<CategoryDTO> CreateCategory(CategoryDTO categoryDto);
        Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDto);
        Task<bool> DeleteCategory(Guid? id);
    }
}