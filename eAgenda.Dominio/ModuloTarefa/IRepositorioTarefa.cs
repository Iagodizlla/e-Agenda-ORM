namespace eAgenda.Dominio.ModuloTarefa;

public interface IRepositorioTarefa
{
    public void CadastrarRegistro(Tarefa tarefa);
    public bool EditarRegistro(Guid idTarefa, Tarefa tarefaEditada);
    public bool ExcluirRegistro(Guid idTarefa);
    public void AdicionarItem(ItemTarefa item);
    public bool AtualizarItem(ItemTarefa itemAtualizado);
    public bool RemoverItem(ItemTarefa item);
    public List<Tarefa> SelecionarRegistros();
    public List<Tarefa> SelecionarTarefasPendentes();
    public List<Tarefa> SelecionarTarefasConcluidas();
    public Tarefa? SelecionarRegistroPorId(Guid idTarefa);
};