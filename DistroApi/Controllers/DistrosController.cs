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
    public class DistrosController : ControllerBase
    {
        private readonly IDistroRepository _repository;
        private readonly AppDbContext _context;

        public DistrosController(IDistroRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> GetDistros()
        {
            var distros = await _repository.GetDistrosWithCategoriasAsync();
            return Ok(distros.ToDistroDTOList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistroDTO>> GetDistro(Guid id)
        {
            var distro = await _repository.GetAsync(d => d.DistroId == id);

            if (distro == null) return NotFound();

            return Ok(distro.ToDistroDTO());
        }

        [HttpPost]
        public async Task<ActionResult<DistroDTO>> PostDistro(DistroDTO dto)
        {
            var distro = dto.ToDistro();

            _repository.Create(distro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDistro),
                new { id = distro.DistroId },
                distro.ToDistroDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistro(Guid id, DistroDTO dto)
        {
            if (id != dto.DistroId) return BadRequest();

            var distro = dto.ToDistro();

            _repository.Update(distro);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _repository.GetAsync(d => d.DistroId == id);
                if (exists == null) return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistro(Guid id)
        {
            var distro = await _repository.GetAsync(d => d.DistroId == id);
            if (distro == null) return NotFound();

            _repository.Delete(distro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
