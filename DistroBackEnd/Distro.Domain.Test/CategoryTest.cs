using System;
using Distro.Domain.Entities;
using Distro.Domain.Validation;
using Xunit;

namespace Distro.Domain.Test;

public class CategoryTest
{
    //Testa a criação com dados válidos
    [Fact]
    public void CreateCategory_WithValidParameters_ResultObjectValidState()
    {
        // Act
        var ex = Record.Exception(() =>
            new Category(Guid.NewGuid(), "Linux Distros"));

        // Assert
        Assert.Null(ex);
    }


    //Testa comportamento ao passar ID vazio ou nulo.
    [Fact]
    public void CreateCategory_WithEmptyId_ThrowsDomainException()
    {
        // Act
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Category(Guid.Empty, "Linux Distros"));

        // Assert
        Assert.Equal("Invalid Id value.", ex.Message);
    }



    //Testa nome muito curto.
    [Fact]
    public void CreateCategory_WithShortName_ThrowsDomainException()
    {
        // Act
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Category(Guid.NewGuid(), "Li"));

        // Assert
        Assert.Equal("Nome inválido. Nome deve ter no mínimo 3 caracteres.", ex.Message);
    }



    //Testa nome vazio ou nulo.
    [Fact]
    public void CreateCategory_WithEmptyName_ThrowsDomainException()
    {
        // Act
        var ex = Assert.Throws<DomainExceptionValidation>(() =>
            new Category(Guid.NewGuid(), ""));

        // Assert
        Assert.Equal("Nome inválido. Nome é obrigatório.", ex.Message);
    }

    [Fact]
    public void CreateCategory_WithoutId_WithValidParameters_ResultObjectValidState()
    {
        var ex = Record.Exception(() => new Category("Linux Distros"));
        Assert.Null(ex);
    }

    [Fact]
    public void CreateCategory_WithoutId_WithEmptyName_ThrowsDomainException()
    {
        var ex = Assert.Throws<DomainExceptionValidation>(() => new Category(""));
        Assert.Equal("Nome inválido. Nome é obrigatório.", ex.Message);
    }

    [Fact]
    public void UpdateCategory_WithEmptyName_ThrowsDomainException()
    {
        var category = new Category(Guid.NewGuid(), "Linux Distros");
        var ex = Assert.Throws<DomainExceptionValidation>(() => category.Update(""));
        Assert.Equal("Nome inválido. Nome é obrigatório.", ex.Message);
    }



}
