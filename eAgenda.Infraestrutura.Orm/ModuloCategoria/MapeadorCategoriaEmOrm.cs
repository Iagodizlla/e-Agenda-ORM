using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloDespesa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloCategoria;

public class MapeadorCategoriaEmOrm : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(c => c.Despesas)
            .WithMany(d => d.Categorias)
            .UsingEntity<Dictionary<string, object>>(
                "DespesaCategoria",
                j => j
                    .HasOne<Despesa>()
                    .WithMany()
                    .HasForeignKey("DespesaId")
                    .HasConstraintName("FK_DespesaCategoria_Despesa")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Categoria>()
                    .WithMany()
                    .HasForeignKey("CategoriaId")
                    .HasConstraintName("FK_DespesaCategoria_Categoria")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("DespesaId", "CategoriaId");
                });
    }
}