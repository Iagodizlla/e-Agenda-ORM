using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloCategoria;

public class RepositorioCategoriaEmOrm : IRepositorioCategoria
{
    private readonly DbSet<Categoria> registros;

    public RepositorioCategoriaEmOrm(eAgendaDbContext contexto)
    {
        registros = contexto.Set<Categoria>();
    }

    public void CadastrarRegistro(Categoria novoRegistro)
    {
        registros.Add(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Categoria registroEditado)
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

    public virtual Categoria? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public virtual List<Categoria> SelecionarRegistros()
    {
        foreach (var registro in registros.ToList())
            CarregarDespesas(registro);

        return registros.ToList();
    }
    private void CarregarDespesas(Categoria categoria)
    {
        var despesas = registros
            .Include(c => c.Despesas)
            .FirstOrDefault(c => c.Id == categoria.Id)?
            .Despesas;

        if (despesas is not null)
            categoria.Despesas = despesas.ToList();

        return;
    }
}