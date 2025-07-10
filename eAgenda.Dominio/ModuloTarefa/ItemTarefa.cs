namespace eAgenda.Dominio.ModuloTarefa;

public class ItemTarefa
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public bool Concluido { get; set; }
    public Tarefa Tarefa { get; set; }

    public ItemTarefa() { }

    public ItemTarefa(string titulo, Tarefa tarefa) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Tarefa = tarefa;
        Concluido = false;
    }

    public void Concluir()
    {
        Concluido = true;
    }

    public void MarcarPendente()
    {
        Concluido = false;
    }
}