using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Distro.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DistroController : ControllerBase
    {
        private readonly IDistroService _distroService;

        public DistroController(IDistroService distroService)
        {
            _distroService = distroService;
        }

        // GET: api/Distro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> GetAll()
        {
            var distros = await _distroService.GetDistros();
            return Ok(distros);
        }

        // GET: api/Distro/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DistroDTO>> GetById(Guid id)
        {
            var distro = await _distroService.GetDistroById(id);

            if (distro == null)
                return NotFound("Distro não encontrada.");

            return Ok(distro);
        }

        // POST: api/Distro
        [HttpPost]
        public async Task<ActionResult<DistroDTO>> Create([FromBody] DistroDTO distroDto)
        {
            if (distroDto == null)
                return BadRequest("Dados inválidos.");

            var createdDistro = await _distroService.CreateDistro(distroDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDistro.DistroId },
                createdDistro
            );
        }

        // PUT: api/Distro/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DistroDTO>> Update(Guid id, [FromBody] DistroDTO distroDto)
        {
            if (distroDto == null || distroDto.DistroId != id)
                return BadRequest("Dados inconsistentes.");

            var updatedDistro = await _distroService.UpdateDistro(distroDto);

            if (updatedDistro == null)
                return NotFound("Distro não encontrada.");

            return Ok(updatedDistro);
        }

        // DELETE: api/Distro/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _distroService.DeleteDistro(id);

            if (!result)
                return NotFound("Distro não encontrada.");

            return NoContent();
        }
    }
}
