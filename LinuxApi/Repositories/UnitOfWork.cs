

using LinuxApi.Context;
using LinuxApi.Repositories.Interfaces;

namespace LinuxApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDistroRepository? _distroRepo;
        private ICategoriaRepository? _categoriaRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }




        public IDistroRepository DistroRepository
        {
            get
            {
                if (_distroRepo == null)
                {
                    _distroRepo = new DistroRepository(_context);
                }
                return _distroRepo;
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                if (_categoriaRepo == null)
                {
                    _categoriaRepo = new CategoriaRepository(_context);
                }
                return _categoriaRepo;
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}