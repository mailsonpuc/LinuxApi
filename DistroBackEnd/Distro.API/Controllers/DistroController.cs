using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Distro.API.DTOs;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Infra.IoC.Pagination;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _environment;
        private const long MaxImageFileSizeBytes = 1 * 1024 * 1024; // permitido apenas 1 MB da imagem

        public DistroController(IDistroService distroService, IWebHostEnvironment environment)
        {
            _distroService = distroService;
            _environment = environment;
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





        /// <summary>
        /// Pesquisa distribuições com base em critérios.
        /// </summary>
        
        // GET: api/Distro/search?nome=ubuntu
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> Search([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Parâmetro 'nome' é obrigatório para a busca.");

            var distros = await _distroService.FindDistrosByName(nome);

            if (distros == null || !distros.Any())
                return NotFound("Nenhuma distro encontrada com esse critério.");

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
        ///   Requer autenticação. Extensões da imagem aceitas apenas; .jpg, .jpeg, .png, .webp - maxima 1 MB.
        /// </summary>
        /// <response code="401">Usuário não autenticado.</response>

        // POST: api/Distro
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DistroDTO>> Create([FromForm] DistroCreateDTO distroDto)
        {
            if (distroDto == null)
                return BadRequest("Dados inválidos.");

            if (distroDto.ImageFile == null || distroDto.ImageFile.Length == 0)
                return BadRequest("Arquivo de imagem é obrigatório.");

            if (distroDto.ImageFile.Length > MaxImageFileSizeBytes)
                return BadRequest("O arquivo de imagem deve ter no máximo 1 MB.");

            // 1. Validar extensão
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(distroDto.ImageFile.FileName).ToLower();
            
            if (string.IsNullOrEmpty(extension) || !Array.Exists(allowedExtensions, ext => ext == extension))
                return BadRequest("Formato de imagem não permitido. Extensões aceitas: .jpg, .jpeg, .png, .webp");

            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var imagesFolder = Path.Combine(webRoot, "images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(imagesFolder, fileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await distroDto.ImageFile.CopyToAsync(fileStream);
            }

            var imageUrl = $"/images/{fileName}";

            var createDto = new DistroDTO
            {
                ImageUrl = imageUrl,
                Nome = distroDto.Nome,
                Descricao = distroDto.Descricao,
                Iso = distroDto.Iso,
                CategoryId = distroDto.CategoryId
            };

            var createdDistro = await _distroService.CreateDistro(createDto);

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
        [Authorize]
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
        [Authorize]
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
