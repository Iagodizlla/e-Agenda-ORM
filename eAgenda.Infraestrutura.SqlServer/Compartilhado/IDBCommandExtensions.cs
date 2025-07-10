using System.Data;

namespace eAgenda.Infraestrutura.SqlServer.Compartilhado;

public static class IDbCommandExtensions
{
    public static void AdicionarParametro(
        this IDbCommand comando,
        string nome,
        object valor
    )
    {
        var parametro = comando.CreateParameter();
        parametro.ParameterName = nome;
        parametro.Value = valor;

        comando.Parameters.Add(parametro);
    }
}