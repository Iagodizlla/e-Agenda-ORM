using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloDespesa;

public class MapeadorDespesaEmOrm : IEntityTypeConfiguration<Despesa>
{
    public void Configure(EntityTypeBuilder<Despesa> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DataOcorencia)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.FormaPagamento)
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(d => d.Categorias)
            .WithMany(c => c.Despesas);
    }
}