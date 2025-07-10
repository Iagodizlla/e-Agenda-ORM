using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.WebApp.Models;

public class FormularioContatoViewModel
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo \"Telefone\" é obrigatório.")]
    [RegularExpression(@"^\(?\d{2}\)?\s?(9\d{4}|\d{4})-?\d{4}$", ErrorMessage = "O campo \"Telefone\" precisa estar no formato (DDD) 90000-0000")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O campo \"Email\" é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo \"Email\" precisa estar no formato nome@provedor.com.")]
    public string Email { get; set; }

    public string? Empresa { get; set; }
    public string? Cargo { get; set; }
}

public class CadastrarContatoViewModel : FormularioContatoViewModel
{
    public CadastrarContatoViewModel() { }

    public CadastrarContatoViewModel(string titulo) : this()
    {
        Nome = titulo;
    }
}

public class EditarContatoViewModel : FormularioContatoViewModel
{
    public Guid Id { get; set; }

    public EditarContatoViewModel() { }

    public EditarContatoViewModel(
        Guid id,
        string nome,
        string telefone,
        string email,
        string? empresa,
        string? cargo
    ) : this()
    {
        Id = id;
        Nome = nome;
        Telefone = telefone;
        Email = email;
        Empresa = empresa;
        Cargo = cargo;
    }
}

public class ExcluirContatoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public ExcluirContatoViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarContatosViewModel
{
    public List<DetalhesContatoViewModel> Registros { get; set; }

    public VisualizarContatosViewModel(List<Contato> contatos)
    {
        Registros = new List<DetalhesContatoViewModel>();

        foreach (var c in contatos)
            Registros.Add(c.ParaDetalhesVM());
    }
}

public class DetalhesContatoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string? Empresa { get; set; }
    public string? Cargo { get; set; }

    public DetalhesContatoViewModel(
        Guid id,
        string nome,
        string telefone,
        string email,
        string? empresa,
        string? cargo
    )
    {
        Id = id;
        Nome = nome;
        Telefone = telefone;
        Email = email;
        Empresa = empresa;
        Cargo = cargo;
    }
}