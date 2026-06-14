using Distro.Domain.Entities;

namespace Distro.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    IQueryable<Category> GetAllCategoriesAsQueryable();
    Task<Category> GetCategoryByIdAsync(Guid categoryId);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Guid categoryId);
}
