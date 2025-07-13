using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloCompromisso;

public class MapeadorCompromissoEmOrm : IEntityTypeConfiguration<Compromisso>
{
    public void Configure(EntityTypeBuilder<Compromisso> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Assunto)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Data)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(x => x.HoraInicio)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.HoraTermino)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Local)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Link)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.HasOne(c => c.Contato)
            .WithMany(con => con.Compromissos)
            .IsRequired(false);
    }
}