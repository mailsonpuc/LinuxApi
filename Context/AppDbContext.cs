using LinuxApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinuxApi.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // Construtor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        //mapeamento
        public DbSet<Distro> Distros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        //fluent api
        protected override void OnModelCreating(ModelBuilder mb)
        {
            // MUITO IMPORTANTE → garante a configuração do Identity
            base.OnModelCreating(mb);

            // Config Distro
            mb.Entity<Distro>().HasKey(c => c.DistroId);

            mb.Entity<Distro>()
                .Property(n => n.Nome).HasMaxLength(50).IsRequired();

            mb.Entity<Distro>()
                .Property(n => n.Descricao).HasMaxLength(100).IsRequired();

            mb.Entity<Distro>()
                .Property(n => n.Iso).HasMaxLength(400).IsRequired();

            // Config Categoria
            mb.Entity<Categoria>().HasKey(c => c.CategoriaId);

            mb.Entity<Categoria>()
                .Property(n => n.Nome).HasMaxLength(100).IsRequired();

            // Relacionamento 1:N
            mb.Entity<Categoria>()
                .HasMany(c => c.Distros)           // Categoria TEM muitas distros
                .WithOne(d => d.Categoria)         // Distro TEM uma categoria
                .HasForeignKey(d => d.CategoriaId) // FK em Distro
                .IsRequired();
        }
    }
}
