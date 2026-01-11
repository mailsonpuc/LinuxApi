

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distro.Domain.Entities;
using Distro.Domain.Interfaces;
using Distro.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Distro.Infra.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }



        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }



        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
