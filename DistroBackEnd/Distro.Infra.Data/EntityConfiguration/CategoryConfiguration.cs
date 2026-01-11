
using System;
using Distro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distro.Infra.Data.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new { CategoryId = Guid.Parse("a1b2c3d4-e5f6-4789-9012-abcdefabcdef"), Name = "Customizaveis" },
                new { CategoryId = Guid.Parse("b2c3d4e5-f678-4901-2345-bcdefabcdefa"), Name = "Privacidade" },
                new { CategoryId = Guid.Parse("89012345-6789-4678-9012-bcdefabcdefa"), Name = "Educacional" },
                new { CategoryId = Guid.Parse("90123456-7890-4789-0123-cdefabcdefab"), Name = "Desenvolvimento" }
            
            );
        }



    }
}