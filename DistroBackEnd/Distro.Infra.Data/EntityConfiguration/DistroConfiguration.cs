using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Distro.Domain.Entities;

namespace Distro.Infra.Data.EntityConfiguration
{
    public class DistroConfiguration : IEntityTypeConfiguration<Distro.Domain.Entities.Distro>
    {
        public void Configure(EntityTypeBuilder<Distro.Domain.Entities.Distro> builder)
        {
            builder.ToTable("Distros");

            builder.HasKey(d => d.DistroId);

            builder.Property(d => d.DistroId)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.ImageUrl)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(d => d.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(d => d.Iso)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.CategoryId)
                .IsRequired();

            builder.HasOne(d => d.Category)
                .WithMany(c => c.Distros)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}