using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.WebApp.Models;

public class FormularioCompromissoViewModel
{
    [Required(ErrorMessage = "O campo \"Assunto\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Assunto\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Assunto\" precisa conter no máximo 100 caracteres.")]
    public string Assunto { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo \"Data\" é obrigatório.")]
    public DateTime Data { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "O campo \"Hora de Início\" é obrigatório.")]
    public TimeSpan HoraInicio { get; set; }

    [Required(ErrorMessage = "O campo \"Hora de Término\" é obrigatório.")]
    public TimeSpan HoraTermino { get; set; }

    [Required(ErrorMessage = "O campo \"Tipo\" é obrigatório.")]
    public TipoCompromisso Tipo { get; set; }

    public string? Local { get; set; }
    public string? Link { get; set; }

    public Guid? ContatoId { get; set; }
    public List<SelectListItem>? ContatosDisponiveis { get; set; }
}

public class CadastrarCompromissoViewModel : FormularioCompromissoViewModel
{
    public CadastrarCompromissoViewModel()
    {
        ContatosDisponiveis = new List<SelectListItem>();
    }

    public CadastrarCompromissoViewModel(List<Contato> contatos) : this()
    {
        foreach (var c in contatos)
        {
            var selecionarVM = new SelectListItem(c.Nome, c.Id.ToString());

            ContatosDisponiveis?.Add(selecionarVM);
        }
    }
}

public class EditarCompromissoViewModel : FormularioCompromissoViewModel
{
    public Guid Id { get; set; }

    public EditarCompromissoViewModel()
    {
        ContatosDisponiveis = new List<SelectListItem>();
    }

    public EditarCompromissoViewModel(
        Guid id,
        string assunto,
        DateTime data,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        TipoCompromisso tipo,
        string? local,
        string? link,
        Guid? contatoId,
        List<Contato> contatos
    ) : this()
    {
        Id = id;
        Assunto = assunto;
        Data = data;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Local = local;
        Link = link;
        ContatoId = contatoId;

        foreach (var c in contatos)
        {
            var selecionarVM = new SelectListItem(c.Nome, c.Id.ToString());

            ContatosDisponiveis?.Add(selecionarVM);
        }
    }
}

public class ExcluirCompromissoViewModel
{
    public Guid Id { get; set; }
    public string Assunto { get; set; }

    public ExcluirCompromissoViewModel(Guid id, string assunto)
    {
        Id = id;
        Assunto = assunto;
    }
}

public class VisualizarCompromissosViewModel
{
    public List<DetalhesCompromissoViewModel> Registros { get; set; }

    public VisualizarCompromissosViewModel(List<Compromisso> compromissos)
    {
        Registros = new List<DetalhesCompromissoViewModel>();

        foreach (var c in compromissos)
            Registros.Add(c.ParaDetalhesVM());
    }
}

public class DetalhesCompromissoViewModel
{
    public Guid Id { get; set; }
    public string Assunto { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string? Local { get; set; }
    public string? Link { get; set; }
    public string? Contato { get; set; }

    public DetalhesCompromissoViewModel(
        Guid id,
        string assunto,
        DateTime data,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        TipoCompromisso tipo,
        string? local,
        string? link,
        string? contato
    )
    {
        Id = id;
        Assunto = assunto;
        Data = data;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Local = local;
        Link = link;
        Contato = contato;
    }
}