using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloCompromisso;

namespace eAgenda.Dominio.ModuloContato
{
    public class Contato : EntidadeBase<Contato>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string? Cargo { get; set; }
        public string? Empresa { get; set; }
        public List<Compromisso> Compromissos { get; set; }

        public Contato(){
            Compromissos = new List<Compromisso>();
        }   
        public Contato(string nome, string email, string telefone, string? cargo, string? empresa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Cargo = cargo;
            Empresa = empresa;
        }
        public override void AtualizarRegistro(Contato registroEditado)
        {
            Nome = registroEditado.Nome;
            Email = registroEditado.Email;
            Telefone = registroEditado.Telefone;
            Cargo = registroEditado.Cargo;
            Empresa = registroEditado.Empresa;
        }
        public void RegistrarCompromisso(Compromisso compromisso)
        {
            if (Compromissos.Any(c => c.Id == compromisso.Id))
                return;

            Compromissos.Add(compromisso);
        }

        public void RemoverCompromisso(Compromisso compromisso)
        {
            if (!Compromissos.Any(c => c.Id == compromisso.Id))
                return;

            Compromissos.Remove(compromisso);
        }
    }
}
