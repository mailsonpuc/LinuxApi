using LinuxApi.DTOS;
using LinuxApi.DTOS.Mappings;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<DistroDTO>> Get()
        {
            var distros = _uof.DistroRepository.GetAll();
            var distrosDto = distros.ToDistroDTOList();
            return Ok(distrosDto);
        }

        // GET BY ID
        [HttpGet("{id:guid}", Name = "ObterDistro")]
        public ActionResult<DistroDTO> Get(Guid id)
        {
            var distro = _uof.DistroRepository.Get(c => c.DistroId == id);
            if (distro is null)
            {
                _logger.LogInformation($"Distro com id={id} não encontrada.");
                return NotFound($"Distro com id={id} não encontrada.");
            }

            return Ok(distro.ToDistroDTO());
        }

        // POST
        [HttpPost]
        public ActionResult<DistroDTO> Post([FromBody] DistroDTO distroDto)
        {
            if (distroDto is null)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var distro = distroDto.ToDistro();
            var distroCriada = _uof.DistroRepository.Create(distro);
            _uof.Commit();

            var distroCriadaDto = distroCriada.ToDistroDTO();

            return CreatedAtRoute("ObterDistro", new { id = distroCriadaDto.DistroId }, distroCriadaDto);
        }

        // PUT
        [HttpPut("{id:guid}")]
        public ActionResult<DistroDTO> Put(Guid id, DistroDTO distroDto)
        {
            if (id != distroDto.DistroId)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var distro = distroDto.ToDistro();
            _uof.DistroRepository.Update(distro);
            _uof.Commit();

            return Ok(distro.ToDistroDTO());
        }

        // DELETE
        [HttpDelete("{id:guid}")]
        public ActionResult<DistroDTO> Delete(Guid id)
        {
            var distro = _uof.DistroRepository.Get(c => c.DistroId == id);
            if (distro is null)
            {
                _logger.LogInformation($"Distro com id={id} não encontrada.");
                return NotFound($"Distro com id={id} não encontrada.");
            }

            var distroExcluida = _uof.DistroRepository.Delete(distro);
            _uof.Commit();

            return Ok(distroExcluida.ToDistroDTO());
        }
    }
}
