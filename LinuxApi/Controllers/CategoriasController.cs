using LinuxApi.DTOS;
using LinuxApi.DTOS.Mappings;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinuxApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        // GET ALL
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);
        }

        // GET BY ID
        [HttpGet("{id:guid}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(Guid id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            return Ok(categoria.ToCategoriaDTO());
        }

        // POST
        [HttpPost]
        public ActionResult<CategoriaDTO> Post([FromBody] CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var categoria = categoriaDto.ToCategoria();
            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var categoriaCriadaDto = categoriaCriada.ToCategoriaDTO();

            return CreatedAtRoute("ObterCategoria", new { id = categoriaCriadaDto.CategoriaId }, categoriaCriadaDto);
        }

        // PUT
        [HttpPut("{id:guid}")]
        public ActionResult<CategoriaDTO> Put(Guid id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var categoria = categoriaDto.ToCategoria();
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria.ToCategoriaDTO());
        }

        // DELETE
        [HttpDelete("{id:guid}")]
        public ActionResult<CategoriaDTO> Delete(Guid id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            return Ok(categoriaExcluida.ToCategoriaDTO());
        }
    }
}
