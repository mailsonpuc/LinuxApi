using System;
using Distro.Domain.Entities;
using Distro.Domain.Validation;
using Xunit;

namespace Distro.Domain.Test;

public class DistroTest
{
    [Fact]
    public void CreateDistro_WithValidParameters_ResultObjectValidState()
    {
        // Act
        var ex = Record.Exception(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        // Assert
        Assert.Null(ex);
    }

    [Fact]
    public void CreateDistro_WithEmptyId_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.Empty,
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("Invalid Id value.", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithShortName_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "Ub",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("Nome inválido. Nome deve ter no mínimo 3 caracteres", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithEmptyName_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("Nome inválido. Nome é obrigatório", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithShortDescription_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "DistroName",
                "Curta",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("Descrição inválida. Descrição deve ter no mínimo 10 caracteres", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithEmptyDescription_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "DistroName",
                "",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("Descrição inválida. Descrição é obrigatória", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithEmptyIso_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "",
                Guid.NewGuid()
            ));

        Assert.Equal("ISO inválido. ISO é obrigatório", ex.Message);
    }


    [Fact]
    public void CreateDistro_WithToLongImageUrl_ThrowsDomainException()
    {
        var longImageUrl = new string('a', 251); // 251 caracteres

        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                longImageUrl,
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("ImageUrl inválido. ImageUrl deve ter no máximo 250 caracteres", ex.Message);
    }


    [Fact]
    public void CreateDistro_WithEmptyImageUrl_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Equal("ImageUrl inválido. ImageUrl é obrigatório", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithoutId_WithValidParameters_ResultObjectValidState()
    {
        var ex = Record.Exception(() =>
            new Distro.Domain.Entities.Distro(
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.NewGuid()
            ));

        Assert.Null(ex);
    }

    [Fact]
    public void CreateDistro_WithoutId_WithEmptyCategoryId_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.Empty
            ));

        Assert.Equal("Categoria inválida. CategoryId é obrigatório", ex.Message);
    }

    [Fact]
    public void CreateDistro_WithEmptyCategoryId_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Distro.Domain.Entities.Distro(
                Guid.NewGuid(),
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.Empty
            ));

        Assert.Equal("Categoria inválida. CategoryId é obrigatório", ex.Message);
    }

    [Fact]
    public void UpdateDistro_WithEmptyCategoryId_ThrowsDomainException()
    {
        var distro = new Distro.Domain.Entities.Distro(
            Guid.NewGuid(),
            "https://example.com/image.png",
            "DistroName",
            "Uma distribuição Linux poderosa e versátil",
            "ubuntu-22.04.iso",
            Guid.NewGuid()
        );

        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            distro.Update(
                "https://example.com/image.png",
                "DistroName",
                "Uma distribuição Linux poderosa e versátil",
                "ubuntu-22.04.iso",
                Guid.Empty
            ));

        Assert.Equal("Categoria inválida. CategoryId é obrigatório", ex.Message);
    }






    
}
