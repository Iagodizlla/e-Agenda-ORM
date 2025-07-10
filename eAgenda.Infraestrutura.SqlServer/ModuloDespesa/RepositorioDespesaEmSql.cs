using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.SqlServer.ModuloDespesa;

public class RepositorioDespesaEmSql : RepositorioBaseEmSql<Despesa>, IRepositorioDespesa
{
    public RepositorioDespesaEmSql(IDbConnection conexaoComBanco)
    : base(conexaoComBanco)
    {
    }

    protected override string SqlInserir => @"
        INSERT INTO [TBDESPESA]
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

    protected override string SqlEditar => @"
        UPDATE [TBDESPESA]	
        SET
            [DESCRICAO] = @DESCRICAO,
            [VALOR] = @VALOR,
            [DATAOCORRENCIA] = @DATAOCORRENCIA,
            [FORMAPAGAMENTO] = @FORMAPAGAMENTO
        WHERE
            [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBDESPESA]
        WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT 
            [ID], 
            [DESCRICAO],
            [VALOR],
            [DATAOCORRENCIA],
            [FORMAPAGAMENTO]
        FROM 
            [TBDESPESA]
        WHERE
            [ID] = @ID";

    protected override string SqlSelecionarTodos => @"
        SELECT 
            [ID], 
            [DESCRICAO],
            [VALOR],
            [DATAOCORRENCIA],
            [FORMAPAGAMENTO]
        FROM 
            [TBDESPESA]";

    protected string SqlAdicionarCategoriaDespesa => @"
        INSERT INTO [TBDESPESA_TBCATEGORIA]
        (
            [DESPESA_ID],
            [CATEGORIA_ID]
        )
        VALUES
        (
            @DESPESA_ID,
            @CATEGORIA_ID
        )";

    protected string SqlRemoverCategoriasDespesa => @"
        DELETE FROM [TBDESPESA_TBCATEGORIA]
        WHERE 
            [DESPESA_ID] = @DESPESA_ID";

    protected string SqlCarregarCategoriasDespesa => @"
        SELECT
            CAT.[ID],
            CAT.[TITULO]
        FROM
            [TBCATEGORIA] AS CAT INNER JOIN
            [TBDESPESA_TBCATEGORIA] AS DC
        ON
            CAT.[ID] = DC.[CATEGORIA_ID]
        WHERE
            DC.[DESPESA_ID] = @DESPESA_ID";

    public override void CadastrarRegistro(Despesa novoRegistro)
    {
        base.CadastrarRegistro(novoRegistro);
        AdicionarCategorias(novoRegistro);
    }

    public override bool EditarRegistro(Guid idRegistro, Despesa registroEditado)
    {
        var resultado = base.EditarRegistro(idRegistro, registroEditado);

        RemoverCategorias(idRegistro);
        AdicionarCategorias(registroEditado);

        return resultado;
    }

    public override bool ExcluirRegistro(Guid idRegistro)
    {
        RemoverCategorias(idRegistro);

        return base.ExcluirRegistro(idRegistro);
    }

    public override Despesa? SelecionarRegistroPorId(Guid idRegistro)
    {
        var registro = base.SelecionarRegistroPorId(idRegistro);

        if (registro is not null)
            CarregarCategorias(registro);

        return registro;
    }

    public override List<Despesa> SelecionarRegistros()
    {
        var registros = base.SelecionarRegistros();

        foreach (var registro in registros)
            CarregarCategorias(registro);

        return registros;
    }

    protected override void ConfigurarParametrosRegistro(Despesa entidade, IDbCommand comando)
    {
        comando.AdicionarParametro("ID", entidade.Id);
        comando.AdicionarParametro("DESCRICAO", entidade.Descricao);
        comando.AdicionarParametro("VALOR", entidade.Valor);
        comando.AdicionarParametro("DATAOCORRENCIA", entidade.DataOcorencia);
        comando.AdicionarParametro("FORMAPAGAMENTO", (int)entidade.FormaPagamento);
    }

    protected override Despesa ConverterParaRegistro(IDataReader leitor)
    {
        return new Despesa
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Descricao = leitor["DESCRICAO"].ToString()!,
            Valor = Convert.ToDecimal(leitor["VALOR"]),
            DataOcorencia = Convert.ToDateTime(leitor["DATAOCORRENCIA"]),
            FormaPagamento = (FormaPagamento)Convert.ToInt32(leitor["FORMAPAGAMENTO"])
        };
    }

    private Categoria ConverterParaCategoria(IDataReader leitor)
    {
        var registro = new Categoria
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titulo = Convert.ToString(leitor["TITULO"])!
        };

        return registro;
    }

    private void AdicionarCategorias(Despesa despesa)
    {
        conexaoComBanco.Open();

        foreach (var cat in despesa.Categorias)
        {
            var comando = conexaoComBanco.CreateCommand();
            comando.CommandText = SqlAdicionarCategoriaDespesa;

            comando.AdicionarParametro("DESPESA_ID", despesa.Id);
            comando.AdicionarParametro("CATEGORIA_ID", cat.Id);

            comando.ExecuteNonQuery();
        }

        conexaoComBanco.Close();
    }

    private void RemoverCategorias(Guid idDespesa)
    {
        var comando = conexaoComBanco.CreateCommand();
        comando.CommandText = SqlRemoverCategoriasDespesa;

        comando.AdicionarParametro("DESPESA_ID", idDespesa);

        conexaoComBanco.Open();

        comando.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    private void CarregarCategorias(Despesa despesa)
    {
        var comando = conexaoComBanco.CreateCommand();
        comando.CommandText = SqlCarregarCategoriasDespesa;

        comando.AdicionarParametro("DESPESA_ID", despesa.Id);

        conexaoComBanco.Open();

        var leitor = comando.ExecuteReader();

        while (leitor.Read())
        {
            var categoria = ConverterParaCategoria(leitor);

            despesa.RegistarCategoria(categoria);
        }

        conexaoComBanco.Close();
    }
}