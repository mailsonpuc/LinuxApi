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
        //private readonly IRepository<Categoria> _repository;
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }


        [HttpGet("{id:guid}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(Guid id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
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

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();
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

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok(categoria);
        }



        [HttpDelete("{id:guid}")]
        public ActionResult<Categoria> Delete(Guid id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id={id} não encontrada.");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return Ok(categoriaExcluida);
        }

    }
}