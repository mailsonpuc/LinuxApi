using System.Linq.Expressions;
using LinuxApi.Controllers;
using LinuxApi.DTOS;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LinuxApi.Test.CategoriaTest
{
    public class GetCategoriaTest
    {
        private readonly Mock<IUnitOfWork> _mockUof;
        private readonly Mock<ILogger<CategoriasController>> _mockLogger;
        private readonly CategoriasController _controller;

        public GetCategoriaTest()
        {
            _mockUof = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<CategoriasController>>();
            _controller = new CategoriasController(_mockUof.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCategoriaById_OkResult()
        {
            // Arrange
            var categoriaId = Guid.Parse("9042c336-5c1d-473e-deba-08dde421bf14");
            var categoria = new Categoria
            {
                CategoriaId = categoriaId,
                Nome = "Customizaveis"
            };

            _mockUof.Setup(u => u.CategoriaRepository.GetAsync(It.IsAny<Expression<Func<Categoria, bool>>>()))
                    .ReturnsAsync(categoria);

            // Act
            var result = await _controller.Get(categoriaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CategoriaDTO>(okResult.Value);
            Assert.Equal(categoriaId, returnValue.CategoriaId);
        }

        [Fact]
        public async Task GetCategoriaById_Returns_NotFound()
        {
            // Arrange
            var categoriaId = Guid.NewGuid(); 
            
            _mockUof.Setup(u => u.CategoriaRepository.GetAsync(It.IsAny<Expression<Func<Categoria, bool>>>()))
                    .ReturnsAsync((Categoria)null!); // Configura o mock para retornar nulo.

            // Act
            var result = await _controller.Get(categoriaId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Categoria com id={categoriaId} não encontrada.", notFoundResult.Value);
        }
    }
}