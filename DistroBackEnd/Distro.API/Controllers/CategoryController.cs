using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Distro.API.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }



        // GET: api/category/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDTO>> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
                return NotFound("Categoria não encontrada");

            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Dados inválidos");

            var createdCategory = await _categoryService.CreateCategory(categoryDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdCategory.CategoryId },
                createdCategory
            );
        }

        // PUT: api/category/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoryDTO>> Update(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || id != categoryDto.CategoryId)
                return BadRequest("Dados inválidos");

            var updatedCategory = await _categoryService.UpdateCategory(categoryDto);

            if (updatedCategory == null)
                return NotFound("Categoria não encontrada");

            return Ok(updatedCategory);
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteCategory(id);

            if (!result)
                return NotFound("Categoria não encontrada");

            return NoContent();
        }
    }
}
