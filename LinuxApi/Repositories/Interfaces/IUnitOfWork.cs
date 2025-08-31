

namespace LinuxApi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IDistroRepository DistroRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}