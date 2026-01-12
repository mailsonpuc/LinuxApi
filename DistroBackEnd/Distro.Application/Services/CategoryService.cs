using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Domain.Entities;
using Distro.Domain.Interfaces;

namespace Distro.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categoriesEntity = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categoriesEntity);
        }

        public async Task<CategoryDTO> GetCategoryById(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var categoryEntity = await _categoryRepository.GetCategoryByIdAsync(id.Value);

            if (categoryEntity == null)
                return null;

            return _mapper.Map<CategoryDTO>(categoryEntity);
        }

        public async Task<CategoryDTO> CreateCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto));

            var categoryEntity = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.AddCategoryAsync(categoryEntity);

            return _mapper.Map<CategoryDTO>(categoryEntity);
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

            return _mapper.Map<CategoryDTO>(categoryEntity);
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
