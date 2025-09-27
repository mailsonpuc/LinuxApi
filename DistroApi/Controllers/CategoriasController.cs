using DistroApi.Context;
using DistroApi.DTOS;
using DistroApi.DTOS.Mappings;
using DistroApi.Models;
using DistroApi.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly AppDbContext _context; // usando o SaveChangesAsync com AppDbContext

        public CategoriasController(ICategoriaRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var categorias = await _repository.GetAllAsync();
            return Ok(categorias.ToCategoriaoDTOList());
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(Guid id)
        {
            var categoria = await _repository.GetAsync(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria.ToCategoiaDTO());
        }

        // PUT: api/Categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(Guid id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }

            var categoria = categoriaDto.ToCategoria();

            _repository.Update(categoria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _repository.GetAsync(c => c.CategoriaId == id);
                if (exists == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostCategoria(CategoriaDTO categoriaDto)
        {
            var categoria = categoriaDto.ToCategoria();

            _repository.Create(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria),
                new { id = categoria.CategoriaId },
                categoria.ToCategoiaDTO());
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(Guid id)
        {
            var categoria = await _repository.GetAsync(c => c.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            _repository.Delete(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
