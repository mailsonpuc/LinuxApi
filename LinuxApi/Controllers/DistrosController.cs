using LinuxApi.DTOS;
using LinuxApi.DTOS.Mappings;
using LinuxApi.Models;
using LinuxApi.Pagination;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LinuxApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<DistrosController> _logger;

        public DistrosController(IUnitOfWork uof, ILogger<DistrosController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> Get()
        {
            var distros = await _uof.DistroRepository.GetAllAsync();
            var distrosDto = distros.ToDistroDTOList();
            return Ok(distrosDto);
        }

        // GET BY ID
        [HttpGet("{id:guid}", Name = "ObterDistro")]
        public async Task<ActionResult<DistroDTO>> Get(Guid id)
        {
            var distro = await _uof.DistroRepository.GetAsync(c => c.DistroId == id);
            if (distro is null)
            {
                _logger.LogInformation($"Distro com id={id} não encontrada.");
                return NotFound($"Distro com id={id} não encontrada.");
            }

            return Ok(distro.ToDistroDTO());
        }

        //pagination
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> Pagination([FromQuery] DistrosParameters distrosParameters)
        {
            var distros = await _uof.DistroRepository.GetDistrosAsync(distrosParameters);

            return await ObterCategorias(distros);
        }

        //metodo extraido
        #pragma warning disable CS1998
        private async Task<ActionResult<IEnumerable<DistroDTO>>> ObterCategorias(PagedList<Distro> distros)
        {
            var metadata = new
            {
                distros.TotalCount,
                distros.PageSize,
                distros.CurrentPage,
                distros.TotalPages,
                distros.HasNext,
                distros.HasPrevieus
            };

            Response.Headers.Append("X-Pagination-info", JsonConvert.SerializeObject(metadata));
            var distrosDto = distros.ToDistroDTOList();
            return Ok(distrosDto);
        }
        #pragma warning restore CS1998

        //filtro nome
        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<DistroDTO>>> GetDistroFiltradas([FromQuery] DistroFiltroNome distroFiltroNome)
        {
            var distrosFiltradas = await _uof.DistroRepository.GetDistrosFiltroNomeAsync(distroFiltroNome);
            return await ObterCategorias(distrosFiltradas);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<DistroDTO>> Post([FromBody] DistroDTO distroDto)
        {
            if (distroDto is null)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var distro = distroDto.ToDistro();
            var distroCriada = await _uof.DistroRepository.CreateAsync(distro); // CORRIGIDO: Adicionado 'await' e mudado o método
            await _uof.CommitAsync();

            var distroCriadaDto = distroCriada.ToDistroDTO();

            return CreatedAtRoute("ObterDistro", new { id = distroCriadaDto.DistroId }, distroCriadaDto);
        }

        // PUT
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DistroDTO>> Put(Guid id, DistroDTO distroDto)
        {
            if (id != distroDto.DistroId)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var distro = distroDto.ToDistro();
            var distroAtualizada = await _uof.DistroRepository.UpdateAsync(distro); // CORRIGIDO: Adicionado 'await' e mudado o método
            await _uof.CommitAsync();

            return Ok(distroAtualizada.ToDistroDTO());
        }

        // DELETE
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DistroDTO>> Delete(Guid id)
        {
            var distro = await _uof.DistroRepository.GetAsync(c => c.DistroId == id);
            if (distro is null)
            {
                _logger.LogInformation($"Distro com id={id} não encontrada.");
                return NotFound($"Distro com id={id} não encontrada.");
            }

            var distroExcluida = await _uof.DistroRepository.DeleteAsync(distro); // CORRIGIDO: Adicionado 'await' e mudado o método
            await _uof.CommitAsync();

            return Ok(distroExcluida.ToDistroDTO());
        }
    }
}