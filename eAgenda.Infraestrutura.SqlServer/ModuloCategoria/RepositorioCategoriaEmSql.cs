using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using Microsoft.Data.SqlClient;

namespace eAgenda.Infraestrutura.SqlServer.ModuloCategoria;

public class RepositorioCategoriaEmSql : IRepositorioCategoria
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";

    public void CadastrarRegistro(Categoria novoRegistro)
    {
        var sqlInserir =
            @"INSERT INTO [TBCATEGORIA]
            (
                [ID],
                [TITULO]
            )
            VALUES
            (
                @ID,
                @TITULO
            );";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosCategoria(novoRegistro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Categoria registroEditado)
    {
        var sqlEditar =
            @"UPDATE [TBCATEGORIA]	
		    SET
			    [TITULO] = @TITULO
		    WHERE
			    [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosCategoria(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var sqlExcluir =
            @"DELETE FROM [TBCATEGORIA]
		    WHERE
			    [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public Categoria? SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId =
            @"SELECT 
		        [ID], 
		        [TITULO]
	        FROM 
		        [TBCATEGORIA]
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao =
            new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Categoria? registro = null;

        if (leitor.Read())
            registro = ConverterParaCategoria(leitor);

        if (registro is not null)
            CarregarDespesas(registro);

        return registro;
    }

    public List<Categoria> SelecionarRegistros()
    {
        var sqlSelecionarTodos =
            @"SELECT 
		        [ID], 
		        [TITULO]
	        FROM 
		        [TBCATEGORIA]";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        var registros = new List<Categoria>();

        while (leitor.Read())
        {
            var contato = ConverterParaCategoria(leitor);

            registros.Add(contato);
        }

        conexaoComBanco.Close();

        return registros;
    }

    private Categoria ConverterParaCategoria(SqlDataReader leitor)
    {
        var registro = new Categoria
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titulo = Convert.ToString(leitor["TITULO"])!
        };

        return registro;
    }

    private Despesa ConverterParaDespesa(SqlDataReader leitor)
    {
        var registro = new Despesa
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Descricao = Convert.ToString(leitor["DESCRICAO"])!,
            Valor = Convert.ToDecimal(leitor["VALOR"])!,
            DataOcorencia = Convert.ToDateTime(leitor["DATAOCORRENCIA"])!,
            FormaPagamento = (FormaPagamento)leitor["FORMAPAGAMENTO"]!,
        };

        return registro;
    }

    private void ConfigurarParametrosCategoria(Categoria entidade, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", entidade.Id);
        comando.Parameters.AddWithValue("TITULO", entidade.Titulo);
    }

    private void CarregarDespesas(Categoria categoria)
    {
        var sqlSelecionarDespesasDaCategoria =
            @"SELECT
                D.[ID],
                D.[DESCRICAO],
                D.[VALOR],
                D.[DATAOCORRENCIA],
                D.[FORMAPAGAMENTO]
            FROM
                [TBDESPESA] AS D INNER JOIN
                [TBDESPESA_TBCATEGORIA] AS DC
            ON
                D.[ID] = DC.[DESPESA_ID]
            WHERE
                DC.[CATEGORIA_ID] = @CATEGORIA_ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarDespesasDaCategoria, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("CATEGORIA_ID", categoria.Id);

        conexaoComBanco.Open();

        SqlDataReader leitorCategoria = comandoSelecao.ExecuteReader();

        while (leitorCategoria.Read())
        {
            var despesa = ConverterParaDespesa(leitorCategoria);

            despesa.RegistarCategoria(categoria);
        }

        conexaoComBanco.Close();
    }
}