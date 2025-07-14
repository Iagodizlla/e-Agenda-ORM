using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloTarefa;

public class RepositorioTarefaEmOrm : RepositorioBaseEmOrm<Tarefa>, IRepositorioTarefa
{
    private readonly DbSet<Tarefa> registros;
    public RepositorioTarefaEmOrm(eAgendaDbContext contexto) : base(contexto)
    {
        registros = contexto.Set<Tarefa>();
    }

    public void CadastrarRegistro(Tarefa novoRegistro)
    {
        registros.Add(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Tarefa registroEditado)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        return true;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        return true;
    }
     
    public virtual Tarefa? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.Include(r => r.Itens).FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public virtual List<Tarefa> SelecionarRegistros()
    {
        return registros.Include(r => r.Itens).ToList();
    }

    public List<Tarefa> SelecionarTarefasConcluidas()
    {
        var registros = SelecionarRegistros();
        return registros.Where(t => t.Concluida).ToList();
    }

    public List<Tarefa> SelecionarTarefasPendentes()
    {
        var registros = SelecionarRegistros();
        return registros.Where(t => !t.Concluida).ToList();
    }
}