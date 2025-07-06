using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using Microsoft.Data.SqlClient;

namespace eAgenda.Infraestrutura.SqlServer.ModuloDespesa;

public class RepositorioDespesaEmSql : IRepositorioDespesa
{
    private readonly string connectionString =
      "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";

    public void CadastrarRegistro(Despesa novoRegistro)
    {
        var sqlInserir =
            @"INSERT INTO [TBDESPESA]
            (
                [ID],
                [DESCRICAO],
                [VALOR],
                [DATAOCORRENCIA],
                [FORMAPAGAMENTO]
            )
            VALUES
            (
                @ID,
                @DESCRICAO,
                @VALOR,
                @DATAOCORRENCIA,
                @FORMAPAGAMENTO
            );";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosDespesa(novoRegistro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();

        AdicionarCategorias(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Despesa registroEditado)
    {
        var sqlEditar =
            @"UPDATE [TBDESPESA]	
		    SET
			    [DESCRICAO] = @DESCRICAO,
			    [VALOR] = @VALOR,
			    [DATAOCORRENCIA] = @DATAOCORRENCIA,
			    [FORMAPAGAMENTO] = @FORMAPAGAMENTO
		    WHERE
			    [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosDespesa(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        RemoverCategorias(idRegistro);

        AdicionarCategorias(registroEditado);

        return linhasAfetadas > 0;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        RemoverCategorias(idRegistro);

        var sqlExcluir =
            @"DELETE FROM [TBDESPESA]
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

    public Despesa? SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId =
            @"SELECT 
		        [ID], 
		        [DESCRICAO],
		        [VALOR],
		        [DATAOCORRENCIA],
		        [FORMAPAGAMENTO]
	        FROM 
		        [TBDESPESA]
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao =
            new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Despesa? registro = null;

        if (leitor.Read())
            registro = ConverterParaDespesa(leitor);

        if (registro is not null)
            CarregarCategorias(registro);

        return registro;
    }

    public List<Despesa> SelecionarRegistros()
    {
        var sqlSelecionarTodos =
            @"SELECT 
		        [ID], 
		        [DESCRICAO],
		        [VALOR],
		        [DATAOCORRENCIA],
		        [FORMAPAGAMENTO]
	        FROM 
		        [TBDESPESA]";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        var registros = new List<Despesa>();

        while (leitor.Read())
        {
            var contato = ConverterParaDespesa(leitor);

            registros.Add(contato);
        }

        conexaoComBanco.Close();

        foreach (var registro in registros)
            CarregarCategorias(registro);

        return registros;
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

    private Categoria ConverterParaCategoria(SqlDataReader leitor)
    {
        var registro = new Categoria
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titulo = Convert.ToString(leitor["TITULO"])!
        };

        return registro;
    }

    private void ConfigurarParametrosDespesa(Despesa entidade, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", entidade.Id);
        comando.Parameters.AddWithValue("DESCRICAO", entidade.Descricao);
        comando.Parameters.AddWithValue("VALOR", entidade.Valor);
        comando.Parameters.AddWithValue("DATAOCORRENCIA", entidade.DataOcorencia);
        comando.Parameters.AddWithValue("FORMAPAGAMENTO", (int)entidade.FormaPagamento);
    }

    private void AdicionarCategorias(Despesa despesa)
    {
        var sqlAdicionarCategoriaNaDespesa =
            @"INSERT INTO [TBDESPESA_TBCATEGORIA]
            (
                [DESPESA_ID],
                [CATEGORIA_ID]
            )
            VALUES
            (
                @DESPESA_ID,
                @CATEGORIA_ID
            )";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        conexaoComBanco.Open();

        foreach (var cat in despesa.Categorias)
        {
            SqlCommand comandoInsercao = new SqlCommand(sqlAdicionarCategoriaNaDespesa, conexaoComBanco);

            comandoInsercao.Parameters.AddWithValue("DESPESA_ID", despesa.Id);
            comandoInsercao.Parameters.AddWithValue("CATEGORIA_ID", cat.Id);

            comandoInsercao.ExecuteNonQuery();
        }

        conexaoComBanco.Close();
    }

    private void RemoverCategorias(Guid idDespesa)
    {
        var sqlRemoverCategoriaDaDespesa =
            @"DELETE FROM 
                [TBDESPESA_TBCATEGORIA]
            WHERE
                [DESPESA_ID] = @DESPESA_ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlRemoverCategoriaDaDespesa, conexaoComBanco);

        conexaoComBanco.Open();

        comandoExclusao.Parameters.AddWithValue("DESPESA_ID", idDespesa);

        comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    private void CarregarCategorias(Despesa despesa)
    {
        var sqlSelecionarCategoriasDaDespesa =
            @"SELECT
                CAT.[ID],
                CAT.[TITULO]
            FROM
                [TBCATEGORIA] AS CAT INNER JOIN
                [TBDESPESA_TBCATEGORIA] AS DC
            ON
                CAT.[ID] = DC.[CATEGORIA_ID]
            WHERE
                DC.[DESPESA_ID] = @DESPESA_ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarCategoriasDaDespesa, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("DESPESA_ID", despesa.Id);

        conexaoComBanco.Open();

        SqlDataReader leitorCategoria = comandoSelecao.ExecuteReader();

        while (leitorCategoria.Read())
        {
            var categoria = ConverterParaCategoria(leitorCategoria);

            despesa.RegistarCategoria(categoria);
        }

        conexaoComBanco.Close();
    }
}