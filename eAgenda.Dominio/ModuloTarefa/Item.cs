namespace eAgenda.Dominio.ModuloTarefa;

public class Item
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Concluido { get; set; }
    public Tarefa Tarefa { get; set; }

    public Item() { }

    public Item(string titulo, Tarefa tarefa) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Tarefa = tarefa;
        Concluido = "Pendente";
    }

    public Item(string titulo, string concluido, Tarefa tarefa) : this(titulo, tarefa)
    {
        Concluido = concluido;
    }

    public void Concluir()
    {
        Concluido = "Concluída";
    }

    public void MarcarPendente()
    {
        Concluido = "Pendente";
    }
}