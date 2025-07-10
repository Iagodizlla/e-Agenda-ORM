using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloContato;

namespace eAgenda.Dominio.ModuloCompromisso;

public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string? Local { get; set; }
    public string? Link { get; set; }
    public Contato? Contato { get; set; }

    public Compromisso() { }

    public Compromisso(
        string assunto,
        DateTime data,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        TipoCompromisso tipo,
        string? local,
        string? link,
        Contato? contato = null
    ) : this()
    {
        Id = Guid.NewGuid();
        Assunto = assunto;
        Data = data;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Local = local;
        Link = link;
        Contato = contato;
    }

    public override void AtualizarRegistro(Compromisso registroEditado)
    {
        Assunto = registroEditado.Assunto;
        Data = registroEditado.Data;
        HoraInicio = registroEditado.HoraInicio;
        HoraTermino = registroEditado.HoraTermino;
        Tipo = registroEditado.Tipo;
        Local = registroEditado.Local;
        Link = registroEditado.Link;
        Contato = registroEditado.Contato;
    }
}