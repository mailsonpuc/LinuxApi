using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinuxApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrosController : ControllerBase
    {
        private readonly IDistroRepository _DistroRepository;
        private readonly IUnitOfWork _uof;
        private readonly ILogger<DistrosController> _logger;

        public DistrosController(IDistroRepository distroRepository, IUnitOfWork uof, ILogger<DistrosController> logger)
        {
            _DistroRepository = distroRepository;
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Distro>> Get()
        {
            var distros = _uof.DistroRepository.GetAll();
            return Ok(distros);
        }



        [HttpGet("{id:guid}", Name = "ObterDistro")]
        public ActionResult<Distro> Get(Guid id)
        {
            var distro = _uof.DistroRepository.Get(c => c.DistroId == id);
            if (distro is null)
            {
                _logger.LogInformation($"Distro com id={id} não encontrada.");
                return NotFound($"Distro com id={id} não encontrada.");
            }
            return Ok(distro);
        }


        [HttpPost]
        public ActionResult<Distro> Post([FromBody] Distro distro)
        {
            if (distro is null)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var distroCriada = _uof.DistroRepository.Create(distro);
            _uof.Commit();
            return CreatedAtRoute("ObterDistro", new { id = distroCriada.DistroId }, distroCriada);
        }


        [HttpPut("{id:guid}")]
        public ActionResult<Distro> Put(Guid id, Distro distro)
        {
            if (id != distro.DistroId)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            _uof.DistroRepository.Update(distro);
            _uof.Commit();
            return Ok(distro);
        }



        [HttpDelete("{id:guid}")]
        public ActionResult<Distro> Delete(Guid id)
        {
            var distro = _uof.DistroRepository.Get(c => c.DistroId == id);
            if (distro is null)
            {
                _logger.LogInformation($"DIstro com id={id} não encontrada.");
                return NotFound($"Distro com id={id} não encontrada.");
            }

            var DistroExcluida = _uof.DistroRepository.Delete(distro);
            _uof.Commit();
            return Ok(DistroExcluida);
        }


    }
}