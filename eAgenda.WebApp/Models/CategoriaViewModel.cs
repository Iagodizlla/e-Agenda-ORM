using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.WebApp.Models;

public class FormularioCategoriaViewModel
{
    [Required(ErrorMessage = "O campo \"Título\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Título\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Título\" precisa conter no máximo 100 caracteres.")]
    public string? Titulo { get; set; }
}

public class CadastrarCategoriaViewModel : FormularioCategoriaViewModel
{
    public CadastrarCategoriaViewModel() { }

    public CadastrarCategoriaViewModel(string titulo) : this()
    {
        Titulo = titulo;
    }
}

public class EditarCategoriaViewModel : FormularioCategoriaViewModel
{
    public Guid Id { get; set; }

    public EditarCategoriaViewModel() { }

    public EditarCategoriaViewModel(Guid id, string titulo) : this()
    {
        Id = id;
        Titulo = titulo;
    }
}

public class ExcluirCategoriaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }

    public ExcluirCategoriaViewModel(Guid id, string titulo)
    {
        Id = id;
        Titulo = titulo;
    }
}

public class VisualizarCategoriasViewModel
{
    public List<DetalhesCategoriaViewModel> Registros { get; set; }

    public VisualizarCategoriasViewModel(List<Categoria> categorias)
    {
        Registros = new List<DetalhesCategoriaViewModel>();

        foreach (var g in categorias)
            Registros.Add(g.ParaDetalhesVM());
    }
}

public class DetalhesCategoriaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public List<DetalhesDespesaViewModel> Despesas { get; set; }
    public decimal TotalDespesas { get; set; }

    public DetalhesCategoriaViewModel(Guid id, string titulo, List<Despesa> despesas)
    {
        Id = id;
        Titulo = titulo;

        Despesas = new List<DetalhesDespesaViewModel>();

        foreach (var d in despesas)
        {
            TotalDespesas += d.Valor;

            var detalhesDespesaVM = new DetalhesDespesaViewModel(
                d.Id,
                d.Descricao,
                d.Valor,
                d.DataOcorencia,
                d.FormaPagamento,
                d.Categorias
            );

            Despesas.Add(detalhesDespesaVM);
        }
    }
}