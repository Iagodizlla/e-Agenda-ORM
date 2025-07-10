using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.SqlServer.ModuloCategoria;
using eAgenda.Infraestrutura.SqlServer.ModuloCompromisso;
using eAgenda.Infraestrutura.SqlServer.ModuloContato;
using eAgenda.Infraestrutura.SqlServer.ModuloDespesa;
using eAgenda.Infraestrutura.SqlServer.ModuloTarefa;
using eAgenda.WebApp.ActionFilters;
using eAgenda.WebApp.DependencyInjection;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eAgenda.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ValidarModeloAttribute>();
            options.Filters.Add<LogarAcaoAttribute>();
        });

        builder.Services.AddScoped<IDbConnection>(provider =>
        {
            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";

            return new SqlConnection(connectionString);
        });

        builder.Services.AddScoped<IRepositorioContato, RepositorioContatoEmSql>();
        builder.Services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoEmSql>();
        builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmSql>();
        builder.Services.AddScoped<IRepositorioDespesa, RepositorioDespesaEmSql>();
        builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefaEmSql>();

        builder.Services.AddSerilogConfig(builder.Logging);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/erro");
        else
            app.UseDeveloperExceptionPage();

        app.UseAntiforgery();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapDefaultControllerRoute();

        app.Run();
    }
}