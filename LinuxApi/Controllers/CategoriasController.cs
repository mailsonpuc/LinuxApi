using LinuxApi.DTOS;
using LinuxApi.DTOS.Mappings;
using LinuxApi.Models;
using LinuxApi.Pagination;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace LinuxApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;
        private readonly IMemoryCache _cache;
        private const string CacheCategoriasKey = "CacheCategorias";

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger, IMemoryCache cache)
        {
            _uof = uof;
            _logger = logger;
            _cache = cache;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll()
        {
            // Tenta obter os dados do cache.
            if (_cache.TryGetValue(CacheCategoriasKey, out IEnumerable<Categoria> categorias))
            {
                _logger.LogInformation("Categorias obtidas do cache.");
                var categoriasDto = categorias.ToCategoriaDTOList();
                return Ok(categoriasDto);
            }

            // Se o cache não tiver dados, busca do banco de dados.
            _logger.LogInformation("Buscando categorias do banco de dados...");
            categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null || !categorias.Any())
            {
                return NotFound("Nenhuma categoria encontrada.");
            }

            // Adiciona os dados ao cache.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CacheCategoriasKey, categorias, cacheEntryOptions);

            var categoriasDtoSemCache = categorias.ToCategoriaDTOList();
            return Ok(categoriasDtoSemCache);
        }

        // GET BY ID
        [HttpGet("{id:guid}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(Guid id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }
            return Ok(categoria.ToCategoriaDTO());
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Pagination([FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriaParameters);
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevieus
            };
            Response.Headers.Append("X-Pagination-categoria", JsonConvert.SerializeObject(metadata));
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post([FromBody] CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }
            var categoria = categoriaDto.ToCategoria();
            var categoriaCriada = await _uof.CategoriaRepository.CreateAsync(categoria);
            await _uof.CommitAsync();

            // Invalida o cache após uma alteração.
            _cache.Remove(CacheCategoriasKey);

            var categoriaCriadaDto = categoriaCriada.ToCategoriaDTO();
            return CreatedAtRoute("ObterCategoria", new { id = categoriaCriadaDto.CategoriaId }, categoriaCriadaDto);
        }

        // PUT
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoriaDTO>> Put(Guid id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }
            var categoria = categoriaDto.ToCategoria();
            var categoriaAtualizada = await _uof.CategoriaRepository.UpdateAsync(categoria);
            await _uof.CommitAsync();

            // Invalida o cache após uma alteração.
            _cache.Remove(CacheCategoriasKey);

            return Ok(categoriaAtualizada.ToCategoriaDTO());
        }

        // DELETE
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(Guid id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }
            var categoriaExcluida = await _uof.CategoriaRepository.DeleteAsync(categoria);
            await _uof.CommitAsync();

            // Invalida o cache após uma alteração.
            _cache.Remove(CacheCategoriasKey);

            return Ok(categoriaExcluida.ToCategoriaDTO());
        }
    }
}