using eAgenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.SqlServer.ModuloContato;

public class RepositorioContatoEmSql : RepositorioBaseEmSql<Contato>, IRepositorioContato
{
    public RepositorioContatoEmSql(IDbConnection conexaoComBanco) : base(conexaoComBanco)
    {
    }

    protected override string SqlInserir => @"
        INSERT INTO [TBCONTATO]
        (
            [ID],
            [NOME],
            [EMAIL],
            [TELEFONE],
            [EMPRESA],
            [CARGO]
        )
        VALUES
        (
            @ID,
            @NOME,
            @EMAIL,
            @TELEFONE,
            @EMPRESA,
            @CARGO
        );";

    protected override string SqlEditar => @"
        UPDATE [TBCONTATO]	
        SET
            [NOME] = @NOME,
            [EMAIL] = @EMAIL,
            [TELEFONE] = @TELEFONE,
            [EMPRESA] = @EMPRESA,
            [CARGO] = @CARGO
        WHERE
            [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBCONTATO]
        WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT 
            [ID], 
            [NOME], 
            [EMAIL],
            [TELEFONE],
            [EMPRESA],
            [CARGO]
        FROM 
            [TBCONTATO]
        WHERE
            [ID] = @ID";

    protected override string SqlSelecionarTodos => @"
        SELECT 
            [ID], 
            [NOME], 
            [EMAIL],
            [TELEFONE],
            [EMPRESA],
            [CARGO]
        FROM 
            [TBCONTATO]";

    protected override void ConfigurarParametrosRegistro(Contato contato, IDbCommand comando)
    {
        comando.AdicionarParametro("ID", contato.Id);
        comando.AdicionarParametro("NOME", contato.Nome);
        comando.AdicionarParametro("EMAIL", contato.Email);
        comando.AdicionarParametro("TELEFONE", contato.Telefone);
        comando.AdicionarParametro("EMPRESA", contato.Empresa ?? (object)DBNull.Value);
        comando.AdicionarParametro("CARGO", contato.Cargo ?? (object)DBNull.Value);
    }

    protected override Contato ConverterParaRegistro(IDataReader leitor)
    {
        var contato = new Contato
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Nome = Convert.ToString(leitor["NOME"])!,
            Telefone = Convert.ToString(leitor["TELEFONE"])!,
            Email = Convert.ToString(leitor["EMAIL"])!,
            Empresa = Convert.ToString(leitor["EMPRESA"]),
            Cargo = Convert.ToString(leitor["CARGO"])
        };

        return contato;
    }
}