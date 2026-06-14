using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Application.Mappings;
using Distro.Domain.Entities;
using Distro.Domain.Interfaces;
using Distro.Domain.Pagination;

namespace Distro.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categoriesEntity = await _categoryRepository.GetAllCategoriesAsync();
            return categoriesEntity.ToDto();
        }

        public async Task<PagedList<CategoryDTO>> GetCategoriesPaged(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _categoryRepository.GetAllCategoriesAsQueryable();
            var pagedList = await PagedList<Category>.ToPagedListAsync(query, pageNumber, pageSize);
            
            return new PagedList<CategoryDTO>(
                pagedList.Select(c => c.ToDto()).ToList(),
                pagedList.TotalCount,
                pagedList.CurrentPage,
                pagedList.PageSize
            );
        }

        public async Task<CategoryDTO> GetCategoryById(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var categoryEntity = await _categoryRepository.GetCategoryByIdAsync(id.Value);

            if (categoryEntity == null)
                return null;

            return categoryEntity.ToDto();
        }

        public async Task<CategoryDTO> CreateCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto));

            var categoryEntity = categoryDto.ToEntity();

            await _categoryRepository.AddCategoryAsync(categoryEntity);

            return categoryEntity.ToDto();
        }

        public async Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto));

            var categoryEntity = await _categoryRepository.GetCategoryByIdAsync(categoryDto.CategoryId);

            if (categoryEntity == null)
                return null;

            // regra de domínio → update controlado
            categoryEntity.Update(categoryDto.Name);

            await _categoryRepository.UpdateCategoryAsync(categoryEntity);

            return categoryEntity.ToDto();
        }

        public async Task<bool> DeleteCategory(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var categoryEntity = await _categoryRepository.GetCategoryByIdAsync(id.Value);

            if (categoryEntity == null)
                return false;

            await _categoryRepository.DeleteCategoryAsync(id.Value);
            return true;
        }
    }
}
