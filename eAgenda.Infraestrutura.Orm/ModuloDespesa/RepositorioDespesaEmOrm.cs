using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloDespesa;

public class RepositorioDespesaEmOrm : RepositorioBaseEmOrm<Despesa>, IRepositorioDespesa
{
    private readonly DbSet<Despesa> registros;
    public RepositorioDespesaEmOrm(eAgendaDbContext contexto) : base(contexto)
    {
        registros = contexto.Set<Despesa>();
    }

    public void CadastrarRegistro(Despesa novoRegistro)
    {
        registros.Add(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Despesa registroEditado)
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

    public virtual Despesa? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public virtual List<Despesa> SelecionarRegistros()
    {
        return registros.ToList();
    }

    public List<Categoria> CarregarCategorias(Despesa despesa)
    {
        return registros
            .Include(d => d.Categorias)
            .Where(d => d.Id == despesa.Id)
            .SelectMany(d => d.Categorias)
            .ToList();
    }
}
