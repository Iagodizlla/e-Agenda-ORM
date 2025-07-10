using eAgenda.Dominio.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.SqlServer.Compartilhado;

public abstract class RepositorioBaseEmSql<T> where T : EntidadeBase<T>
{
    protected abstract string SqlInserir { get; }
    protected abstract string SqlEditar { get; }
    protected abstract string SqlExcluir { get; }
    protected abstract string SqlSelecionarPorId { get; }
    protected abstract string SqlSelecionarTodos { get; }

    protected readonly IDbConnection conexaoComBanco;

    public RepositorioBaseEmSql(IDbConnection conexaoComBanco)
    {
        this.conexaoComBanco = conexaoComBanco;
    }

    public virtual void CadastrarRegistro(T novoRegistro)
    {
        var comandoInsercao = conexaoComBanco.CreateCommand();
        comandoInsercao.CommandText = SqlInserir;

        ConfigurarParametrosRegistro(novoRegistro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public virtual bool EditarRegistro(Guid idRegistro, T registroEditado)
    {
        var comandoEdicao = conexaoComBanco.CreateCommand();
        comandoEdicao.CommandText = SqlEditar;

        registroEditado.Id = idRegistro;

        ConfigurarParametrosRegistro(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public virtual bool ExcluirRegistro(Guid idRegistro)
    {
        var comandoExclusao = conexaoComBanco.CreateCommand();
        comandoExclusao.CommandText = SqlExcluir;

        comandoExclusao.AdicionarParametro("ID", idRegistro);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public virtual T? SelecionarRegistroPorId(Guid idRegistro)
    {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarPorId;
        comandoSelecao.AdicionarParametro("ID", idRegistro);

        conexaoComBanco.Open();

        var leitor = comandoSelecao.ExecuteReader();

        T? registro = null;

        if (leitor.Read())
            registro = ConverterParaRegistro(leitor);

        conexaoComBanco.Close();

        return registro;
    }

    public virtual List<T> SelecionarRegistros()
    {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarTodos;

        conexaoComBanco.Open();

        var leitor = comandoSelecao.ExecuteReader();

        var registros = new List<T>();

        while (leitor.Read())
        {
            var contato = ConverterParaRegistro(leitor);

            registros.Add(contato);
        }

        conexaoComBanco.Close();

        return registros;
    }

    protected abstract void ConfigurarParametrosRegistro(T registro, IDbCommand comando);

    protected abstract T ConverterParaRegistro(IDataReader leitor);
}