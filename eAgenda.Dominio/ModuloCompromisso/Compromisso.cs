using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloContato;

namespace eAgenda.Dominio.ModuloCompromisso
{
    public enum TipoCompromissoEnum
    {
        Local,
        Remoto
    }

    public class Compromisso : EntidadeBase<Compromisso>
    {

        public string Assunto { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraTermino { get; set; }
        public TipoCompromissoEnum TipoCompromisso { get; set; } 
        public string? Local { get; set; }
        public string? Link { get; set; }
        public Contato? Contato { get; set; }

        public Compromisso() { }

        public Compromisso(string assunto, DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino, TipoCompromissoEnum tipoCompromisso, string local = null, string link = null, Contato? contato = null)
        {
            Id = Guid.NewGuid();
            Assunto = assunto;
            DataOcorrencia = dataOcorrencia;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            TipoCompromisso = tipoCompromisso;
            Local = local;
            Link = link;
            Contato = contato;
        }


        public override void AtualizarRegistro(Compromisso registroEditado)
        {
            Assunto = registroEditado.Assunto;
            DataOcorrencia = registroEditado.DataOcorrencia;
            HoraInicio = registroEditado.HoraInicio;
            HoraTermino = registroEditado.HoraTermino;
            TipoCompromisso = registroEditado.TipoCompromisso;
            Local = registroEditado.Local;
            Link = registroEditado.Link;
            Contato = registroEditado.Contato; // Atualiza o contato, se houver
        }
    }
}
