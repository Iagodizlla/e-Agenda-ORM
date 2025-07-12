
namespace eAgenda.Dominio.Compartilhado
{
    public interface IRepositorio<T> where T : EntidadeBase<T>
    {
        public void CadastrarRegistro(T novoRegistro);
        public bool EditarRegistro(Guid idRegistro, T registroEditado);
        public bool ExcluirRegistro(Guid idRegistro);
        public List<T> SelecionarRegistros();
        public T? SelecionarRegistroPorId(Guid idRegistro);
    }
}
