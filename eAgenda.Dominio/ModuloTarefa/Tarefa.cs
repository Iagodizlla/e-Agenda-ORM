using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.ModuloTarefa;

public class Tarefa : EntidadeBase<Tarefa>
{
    private double percentualConcluida;

    public string Titulo { get; set; }
    public string Prioridade { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataConclusao { get; set; }
    public string StatusConcluida { get; set; } = "Pendente"; // Default value
    public List<Item> Itens { get; set; }

    public double PercentualConcluido
    {
        get
        {
            if (Itens.Count == 0)
                return default;

            int qtdConcluidos = 0;

            foreach (var item in Itens)
            {
                if (item.Concluido != "Pendente")
                    qtdConcluidos++;
            }

            decimal percentualBase = qtdConcluidos / (decimal)Itens.Count * 100;

            return (double)Math.Round(percentualBase, 2);
        }
    }

    public bool Concluida { get; set; }

    public Tarefa()
    {
        Itens = new List<Item>();
    }

    public Tarefa(string titulo, string prioridade) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Prioridade = prioridade;
        Concluida = false;
        DataCriacao = DateTime.Now;
    }

    public Tarefa(string titulo, string prioridade, DateTime dataCriacao, double percentualConcluida, string statusConcluida, DateTime dataConclusao) : this(titulo, prioridade)
    {
        Id = Guid.NewGuid();
        DataCriacao = dataCriacao;
        this.percentualConcluida = percentualConcluida;
        StatusConcluida = statusConcluida;
        DataConclusao = dataConclusao;
    }

    public Tarefa(string titulo, string prioridade, DateTime dataCriacao, string statusConcluida, DateTime dataConclusao) : this(titulo, prioridade)
    {
        Id = Guid.NewGuid();
        DataCriacao = dataCriacao;
        StatusConcluida = statusConcluida;
        DataConclusao = dataConclusao;
    }

    public void Concluir()
    {
        Concluida = true;
        DataConclusao = DateTime.Now;
    }

    public void MarcarPendente()
    {
        Concluida = false;
        DataConclusao = DateTime.MinValue;
    }

    public Item? ObterItem(Guid idItem)
    {
        return Itens.Find(i => i.Id.Equals(idItem));
    }

    public Item AdicionarItem(string titulo)
    {
        var item = new Item(titulo, this);

        Itens.Add(item);

        MarcarPendente();

        return item;
    }

    public Item AdicionarItem(Item item)
    {
        Itens.Add(item);

        return item;
    }

    public bool RemoverItem(Item item)
    {
        Itens.Remove(item);

        MarcarPendente();

        return true;
    }

    public void ConcluirItem(Item item)
    {
        item.Concluir();
    }

    public void MarcarItemPendente(Item item)
    {
        item.MarcarPendente();

        MarcarPendente();
    }

    public override void AtualizarRegistro(Tarefa registroEditado)
    {
        Titulo = registroEditado.Titulo;
        Prioridade = registroEditado.Prioridade;
    }
}