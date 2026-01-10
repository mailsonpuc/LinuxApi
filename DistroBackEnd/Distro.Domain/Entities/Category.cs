using Distro.Domain.Validation;

namespace Distro.Domain.Entities;

public sealed class Category
{
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; }

    // Relação: Categoria possui várias Distros
    public IReadOnlyCollection<Distro> Distros => _distros;
    private readonly List<Distro> _distros = new();

    // Construtor padrão
    public Category(string name)
    {
        ValidateDomain(name);
    }

    // Construtor com Id (ex: para EF ou testes)
    public Category(Guid categoryId, string name)
    {
        DomainExceptionValidation.When(categoryId == Guid.Empty,
            "Invalid Id value.");

        CategoryId = categoryId;
        ValidateDomain(name);
    }

    // Método de atualização
    public void Update(string name)
    {
        ValidateDomain(name);
    }

    // Validação de domínio
    private void ValidateDomain(string name)
    {
        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(name),
            "Nome inválido. Nome é obrigatório.");

        DomainExceptionValidation.When(name.Length < 3,
            "Nome inválido. Nome deve ter no mínimo 3 caracteres.");

        Name = name;
    }
}
