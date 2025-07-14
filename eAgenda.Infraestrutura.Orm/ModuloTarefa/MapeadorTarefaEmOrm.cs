using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloTarefa;

public class MapeadorTarefaEmOrm : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Prioridade)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.DataCriacao)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(x => x.DataConclusao)
            .HasColumnType("datetime2")
            .IsRequired(false);

        builder.Property(x => x.Concluida)
            .HasConversion<bool>()
            .IsRequired();

        builder.HasMany(t => t.Itens)
            .WithOne(i => i.Tarefa);
    }
}