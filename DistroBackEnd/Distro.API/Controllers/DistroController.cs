using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Infra.IoC.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;


namespace Distro.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixedwindow")]
    public class DistroController : ControllerBase
    {
        private readonly IDistroService _distroService;

        public DistroController(IDistroService distroService)
        {
            _distroService = distroService;
        }

        /// <summary>
        /// Obtém todas as distribuições.
        /// </summary>
        /// <returns>Lista de todas as distribuições</returns>
        /// <response code="200">Retorna a lista de todas as distribuições</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> GetAll()
        {
            var distros = await _distroService.GetDistros();
            return Ok(distros);
        }

        /// <summary>
        /// Obtém a lista de distribuições com suporte a paginação usando parâmetros de query avançados.
        /// Retorna informações de paginação no header X-Pagination.
        /// </summary>
        /// <param name="distroParameters">Parâmetros de paginação e filtro.</param>
        /// <returns>Uma coleção de objetos DistroDTO com metadados de paginação.</returns>
        /// <response code="200">Retorna a lista de distribuições com metadados.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet("paginacao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> Paginacao([FromQuery] DistroParameters distroParameters)
        {
            var distros = await _distroService.GetDistrosPaged(distroParameters.PageNumber, distroParameters.PageSize);

            var metadata = new
            {
                distros.TotalCount,
                distros.PageSize,
                distros.CurrentPage,
                distros.TotalPages,
                distros.HasNextPage,
                distros.HasPreviousPage
            };
            
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
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


        /// <summary>
        ///   Requer autenticação.
        /// </summary>
        /// <response code="401">Usuário não autenticado.</response>

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


        /// <summary>
        ///   Requer autenticação.
        /// </summary>
        /// <response code="401">Usuário não autenticado.</response>


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


        /// <summary>
        ///   Requer autenticação.
        /// </summary>
        /// <response code="401">Usuário não autenticado.</response>

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
