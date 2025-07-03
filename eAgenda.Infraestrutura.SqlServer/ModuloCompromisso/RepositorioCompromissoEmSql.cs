using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using Microsoft.Data.SqlClient;

namespace eAgenda.Infraestrutura.SqlServer.ModuloCompromisso;

public class RepositorioCompromissoEmSql : IRepositorioCompromisso
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";

    public void CadastrarRegistro(Compromisso novoRegistro)
    {
        var sqlInserir =
            @"INSERT INTO [TBCOMPROMISSO]
            (
                [ID],
                [ASSUNTO],
                [DATA], 
                [HORAINICIO],                    
                [HORATERMINO],
                [TIPO],  
                [LOCAL],       
                [LINK],            
                [CONTATO_ID]
            )
            VALUES
            (
                @ID,
                @ASSUNTO,
                @DATA,
                @HORAINICIO,
                @HORATERMINO,
                @TIPO,
                @LOCAL,
                @LINK,
                @CONTATO_ID
            );";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosCompromisso(novoRegistro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Compromisso registroEditado)
    {
        var sqlEditar =
            @"UPDATE [TBCOMPROMISSO]
            SET 
                [ASSUNTO] = @ASSUNTO,
                [DATA] = @DATA, 
                [HORAINICIO] = @HORAINICIO, 
                [HORATERMINO] = @HORATERMINO,
                [TIPO] = @TIPO,
                [LOCAL] = @LOCAL, 
                [LINK] = @LINK,
                [CONTATO_ID] = @CONTATO_ID
            WHERE 
                [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosCompromisso(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var numeroRegistrosAfetados = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return numeroRegistrosAfetados > 0;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var sqlExcluir =
            @"DELETE FROM [TBCOMPROMISSO]
		    WHERE
			    [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        var numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return numeroRegistrosExcluidos > 0;
    }

    public Compromisso? SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId =
            @"SELECT
                CP.[ID],
                CP.[ASSUNTO],
                CP.[DATA],
                CP.[HORAINICIO],
                CP.[HORATERMINO],
                CP.[TIPO],
                CP.[LOCAL],
                CP.[LINK],
                CP.[CONTATO_ID],
                CT.[NOME],
                CT.[EMAIL],
                CT.[TELEFONE],
                CT.[CARGO],
                CT.[EMPRESA]
            FROM
                [TBCompromisso] AS CP LEFT JOIN
                [TBContato] AS CT
            ON
                CT.ID = CP.CONTATO_ID
            WHERE
                CP.[ID] = @ID;";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Compromisso? compromisso = null;

        if (leitor.Read())
            compromisso = ConverterParaCompromisso(leitor);

        conexaoComBanco.Close();

        return compromisso;
    }

    public List<Compromisso> SelecionarRegistros()
    {
        var sqlSelecionarTodos =
           @"SELECT
                CP.[ID],
                CP.[ASSUNTO],
                CP.[DATA],
                CP.[HORAINICIO],
                CP.[HORATERMINO],
                CP.[TIPO],
                CP.[LOCAL],
                CP.[LINK],
                CP.[CONTATO_ID],
                CT.[NOME],
                CT.[EMAIL],
                CT.[TELEFONE],
                CT.[CARGO],
                CT.[EMPRESA]
            FROM
                [TBCompromisso] AS CP LEFT JOIN
                [TBContato] AS CT
            ON
                CT.ID = CP.CONTATO_ID;";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

        var compromissos = new List<Compromisso>();

        while (leitorCompromisso.Read())
        {
            var compromisso = ConverterParaCompromisso(leitorCompromisso);

            compromissos.Add(compromisso);
        }

        conexaoComBanco.Close();

        return compromissos;
    }

    private Compromisso ConverterParaCompromisso(SqlDataReader leitorCompromisso)
    {
        var horaInicio = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HORAINICIO"]));
        var horaTermino = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HORATERMINO"]));

        Contato? contato = null;

        if (!leitorCompromisso["CONTATO_ID"].Equals(DBNull.Value))
            contato = ConverterParaContato(leitorCompromisso);

        var compromisso = new Compromisso(
            Convert.ToString(leitorCompromisso["ASSUNTO"])!,
            Convert.ToDateTime(leitorCompromisso["DATA"]),
            horaInicio,
            horaTermino,
            (TipoCompromissoEnum)Convert.ToInt32(leitorCompromisso["TIPO"]),
            Convert.ToString(leitorCompromisso["LOCAL"])!,
            Convert.ToString(leitorCompromisso["LINK"])!,
            contato
        );

        compromisso.Id = Guid.Parse(leitorCompromisso["ID"].ToString()!);

        return compromisso;
    }

    private Contato ConverterParaContato(SqlDataReader leitor)
    {
        var contato = new Contato(
            Convert.ToString(leitor["NOME"])!,
            Convert.ToString(leitor["TELEFONE"])!,
            Convert.ToString(leitor["EMAIL"])!,
            Convert.ToString(leitor["EMPRESA"]),
            Convert.ToString(leitor["CARGO"])
        );

        contato.Id = Guid.Parse(leitor["ID"].ToString()!);

        return contato;
    }

    private void ConfigurarParametrosCompromisso(Compromisso compromisso, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", compromisso.Id);
        comando.Parameters.AddWithValue("ASSUNTO", compromisso.Assunto);
        comando.Parameters.AddWithValue("DATA", compromisso.DataOcorrencia);
        comando.Parameters.AddWithValue("HORAINICIO", compromisso.HoraInicio.Ticks);
        comando.Parameters.AddWithValue("HORATERMINO", compromisso.HoraTermino.Ticks);

        comando.Parameters.AddWithValue("TIPO", (int)compromisso.TipoCompromisso);
        comando.Parameters.AddWithValue("LOCAL", compromisso.Local ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("LINK", compromisso.Link ?? (object)DBNull.Value);

        comando.Parameters.AddWithValue("CONTATO_ID", compromisso.Contato?.Id ?? (object)DBNull.Value);
    }
}