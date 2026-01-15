using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Distro.API.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de categorias de distribuições.
    /// </summary>
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

        /// <summary>
        /// Obtém a lista de todas as categorias cadastradas.
        /// </summary>
        /// <returns>Uma coleção de objetos CategoryDTO.</returns>
        /// <response code="200">Retorna a lista de categorias.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }

        /// <summary>
        /// Obtém uma categoria específica através do seu identificador único (GUID).
        /// </summary>
        /// <param name="id">O identificador único da categoria.</param>
        /// <returns>Os detalhes da categoria solicitada.</returns>
        /// <response code="200">Retorna a categoria encontrada.</response>
        /// <response code="404">Categoria não encontrada.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CategoryDTO>> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
                return NotFound("Categoria não encontrada");

            return Ok(category);
        }

        /// <summary>
        /// Cria uma nova categoria no sistema.
        /// </summary>
        /// <param name="categoryDto">Objeto contendo os dados da nova categoria.</param>
        /// <returns>A categoria recém-criada.</returns>
        /// <response code="201">Categoria criada com sucesso.</response>
        /// <response code="400">Dados fornecidos são inválidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Atualiza os dados de uma categoria existente.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser atualizada.</param>
        /// <param name="categoryDto">Objeto com os novos dados da categoria.</param>
        /// <returns>A categoria atualizada.</returns>
        /// <response code="200">Categoria atualizada com sucesso.</response>
        /// <response code="400">Inconsistência entre o ID da URL e o ID do objeto.</response>
        /// <response code="404">Categoria não encontrada para atualização.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CategoryDTO>> Update(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || id != categoryDto.CategoryId)
                return BadRequest("Dados inválidos");

            var updatedCategory = await _categoryService.UpdateCategory(categoryDto);

            if (updatedCategory == null)
                return NotFound("Categoria não encontrada");

            return Ok(updatedCategory);
        }

        /// <summary>
        /// Remove uma categoria do sistema.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser excluída.</param>
        /// <returns>Resposta sem conteúdo em caso de sucesso.</returns>
        /// <response code="204">Categoria removida com sucesso.</response>
        /// <response code="404">Categoria não encontrada.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteCategory(id);

            if (!result)
                return NotFound("Categoria não encontrada");

            return NoContent();
        }
    }
}