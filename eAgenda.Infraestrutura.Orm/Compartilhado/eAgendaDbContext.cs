using Microsoft.EntityFrameworkCore;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.Orm.ModuloContato;

namespace eAgenda.Infraestrutura.Orm.Compartilhado;

public class eAgendaDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }


    public eAgendaDbContext(DbContextOptions<eAgendaDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MapeadorContatoEmOrm());

        base.OnModelCreating(modelBuilder);
    }
}