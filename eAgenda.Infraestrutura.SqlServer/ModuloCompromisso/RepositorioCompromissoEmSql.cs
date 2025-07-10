using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.SqlServer.ModuloCompromisso;

public class RepositorioCompromissoEmSql : RepositorioBaseEmSql<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromissoEmSql(IDbConnection conexaoComBanco)
        : base(conexaoComBanco)
    {
    }

    protected override string SqlInserir => @"
        INSERT INTO [TBCOMPROMISSO]
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

    protected override string SqlEditar => @"
        UPDATE [TBCOMPROMISSO]
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

    protected override string SqlExcluir => @"
        DELETE FROM [TBCOMPROMISSO]
        WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT
            CP.[ID],
            CP.[ASSUNTO],
            CP.[DATA],
            CP.[HORAINICIO],
            CP.[HORATERMINO],
            CP.[TIPO],
            CP.[LOCAL],
            CP.[LINK],
            CP.[CONTATO_ID],
            CT.[ID] AS CONTATO_ID_COMPLETO,
            CT.[NOME],
            CT.[EMAIL],
            CT.[TELEFONE],
            CT.[CARGO],
            CT.[EMPRESA]
        FROM
            [TBCOMPROMISSO] AS CP LEFT JOIN
            [TBCONTATO] AS CT
        ON
            CT.ID = CP.CONTATO_ID
        WHERE
            CP.[ID] = @ID;";

    protected override string SqlSelecionarTodos => @"
        SELECT
            CP.[ID],
            CP.[ASSUNTO],
            CP.[DATA],
            CP.[HORAINICIO],
            CP.[HORATERMINO],
            CP.[TIPO],
            CP.[LOCAL],
            CP.[LINK],
            CP.[CONTATO_ID],
            CT.[ID] AS CONTATO_ID_COMPLETO,
            CT.[NOME],
            CT.[EMAIL],
            CT.[TELEFONE],
            CT.[CARGO],
            CT.[EMPRESA]
        FROM
            [TBCOMPROMISSO] AS CP LEFT JOIN
            [TBCONTATO] AS CT
        ON
            CT.ID = CP.CONTATO_ID;";

    protected override void ConfigurarParametrosRegistro(Compromisso compromisso, IDbCommand comando)
    {
        comando.AdicionarParametro("ID", compromisso.Id);
        comando.AdicionarParametro("ASSUNTO", compromisso.Assunto);
        comando.AdicionarParametro("DATA", compromisso.Data);
        comando.AdicionarParametro("HORAINICIO", compromisso.HoraInicio.Ticks);
        comando.AdicionarParametro("HORATERMINO", compromisso.HoraTermino.Ticks);
        comando.AdicionarParametro("TIPO", (int)compromisso.Tipo);
        comando.AdicionarParametro("LOCAL", compromisso.Local ?? (object)DBNull.Value);
        comando.AdicionarParametro("LINK", compromisso.Link ?? (object)DBNull.Value);
        comando.AdicionarParametro("CONTATO_ID", compromisso.Contato?.Id ?? (object)DBNull.Value);
    }

    protected override Compromisso ConverterParaRegistro(IDataReader leitorCompromisso)
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
            (TipoCompromisso)Convert.ToInt32(leitorCompromisso["TIPO"]),
            Convert.ToString(leitorCompromisso["LOCAL"]),
            Convert.ToString(leitorCompromisso["LINK"]),
            contato
        );

        compromisso.Id = Guid.Parse(leitorCompromisso["ID"].ToString()!);

        return compromisso;
    }

    private Contato ConverterParaContato(IDataReader leitor)
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
}