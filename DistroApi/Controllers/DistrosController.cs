using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistroApi.Context;
using DistroApi.Models;

namespace DistroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DistrosController(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Obtem distros com categorias.
        /// </summary>
        // GET: api/Distros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Distro>>> GetDistros()
        {
            return await _context.Distros
                .Include(d => d.Categoria) // carrega a categoria junto
                .ToListAsync();
        }


        // GET: api/Distros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Distro>> GetDistro(Guid id)
        {
            var distro = await _context.Distros
                .Include(d => d.Categoria) // inclui categoria no retorno
                .FirstOrDefaultAsync(d => d.DistroId == id);

            if (distro == null)
            {
                return NotFound();
            }

            return distro;
        }



        // PUT: api/Distros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistro(Guid id, Distro distro)
        {
            if (id != distro.DistroId)
            {
                return BadRequest();
            }

            _context.Entry(distro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Distros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Distro>> PostDistro(Distro distro)
        {
            _context.Distros.Add(distro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistro", new { id = distro.DistroId }, distro);
        }

        // DELETE: api/Distros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistro(Guid id)
        {
            var distro = await _context.Distros.FindAsync(id);
            if (distro == null)
            {
                return NotFound();
            }

            _context.Distros.Remove(distro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistroExists(Guid id)
        {
            return _context.Distros.Any(e => e.DistroId == id);
        }
    }
}
