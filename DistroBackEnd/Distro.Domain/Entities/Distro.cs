using Distro.Domain.Validation;

namespace Distro.Domain.Entities;

public sealed class Distro
{
    public Guid DistroId { get; private set; }
    public string ImageUrl { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string Iso { get; private set; }

    // Foreign Key
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }

    // Construtor padrão
    public Distro(string imageUrl, string nome, string descricao, string iso, Guid categoryId)
    {
        ValidateDomain(imageUrl, nome, descricao, iso, categoryId);
    }

    // Construtor com ID (usado pelo EF )
    public Distro(Guid distroId, string imageUrl, string nome, string descricao, string iso, Guid categoryId)
    {
        DomainExceptionValidation.When(distroId == Guid.Empty,
            "Invalid Id value.");

        DistroId = distroId;
        ValidateDomain(imageUrl, nome, descricao, iso, categoryId);
    }

    public void Update(string imageUrl, string nome, string descricao, string iso, Guid categoryId)
    {
        ValidateDomain(imageUrl, nome, descricao, iso, categoryId);
    }

    private void ValidateDomain(
        string imageUrl,
        string nome,
        string descricao,
        string iso,
        Guid categoryId)
    {
        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(imageUrl),
            "ImageUrl inválido. ImageUrl é obrigatório");

        DomainExceptionValidation.When(imageUrl.Length > 250,
            "ImageUrl inválido. ImageUrl deve ter no máximo 250 caracteres");

        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(nome),
            "Nome inválido. Nome é obrigatório");

        DomainExceptionValidation.When(nome.Length < 3,
            "Nome inválido. Nome deve ter no mínimo 3 caracteres");

        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(descricao),
            "Descrição inválida. Descrição é obrigatória");

        DomainExceptionValidation.When(descricao.Length < 10,
            "Descrição inválida. Descrição deve ter no mínimo 10 caracteres");

        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(iso),
            "ISO inválido. ISO é obrigatório");

        DomainExceptionValidation.When(categoryId == Guid.Empty,
            "Categoria inválida. CategoryId é obrigatório");

        ImageUrl = imageUrl;
        Nome = nome;
        Descricao = descricao;
        Iso = iso;
        CategoryId = categoryId;
    }
}
