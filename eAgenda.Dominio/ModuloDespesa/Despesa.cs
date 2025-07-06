using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloCategoria;

namespace eAgenda.Dominio.ModuloDespesa;

public class Despesa : EntidadeBase<Despesa>
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataOcorencia { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public List<Categoria> Categorias { get; set; }

    public Despesa()
    {
        Categorias = new List<Categoria>();
    }

    public Despesa(string descricao, decimal valor, DateTime data, FormaPagamento formaPagamento) : this()
    {
        Id = Guid.NewGuid();
        Descricao = descricao;
        Valor = valor;
        DataOcorencia = data;
        FormaPagamento = formaPagamento;
    }

    public void RegistarCategoria(Categoria categoria)
    {
        if (Categorias.Contains(categoria))
            return;

        categoria.Despesas.Add(this);
        Categorias.Add(categoria);
    }

    public void RemoverCategoria(Categoria categoria)
    {
        if (!Categorias.Contains(categoria))
            return;

        categoria.Despesas.Remove(this);
        Categorias.Remove(categoria);
    }

    public override void AtualizarRegistro(Despesa registroEditado)
    {
        Descricao = registroEditado.Descricao;
        Valor = registroEditado.Valor;
        DataOcorencia = registroEditado.DataOcorencia;
        FormaPagamento = registroEditado.FormaPagamento;
    }
}