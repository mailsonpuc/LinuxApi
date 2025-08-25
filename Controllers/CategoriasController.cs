using System;
using System.Collections.Generic;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinuxApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IRepository<Categoria> _repository;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IRepository<Categoria> repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id:guid}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(Guid id)
        {
            var categoria = _repository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post([FromBody] Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var categoriaCriada = _repository.Create(categoria);
            return CreatedAtRoute("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }


        [HttpPut("{id:guid}")]
        public ActionResult<Categoria> Put(Guid id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogInformation("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            _repository.Update(categoria);
            return Ok(categoria);
        }



        [HttpDelete("{id:guid}")]
        public ActionResult<Categoria> Delete(Guid id)
        {
            var categoria = _repository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            var categoriaExcluida = _repository.Delete(categoria);
            return Ok(categoriaExcluida);
        }

    }
}