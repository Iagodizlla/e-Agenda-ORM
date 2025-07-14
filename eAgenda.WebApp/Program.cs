using ControleDeBar.WebApp.DependencyInjection;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.Orm.ModuloCategoria;
using eAgenda.Infraestrutura.Orm.ModuloCompromisso;
using eAgenda.Infraestrutura.Orm.ModuloContato;
using eAgenda.Infraestrutura.Orm.ModuloDespesa;
using eAgenda.Infraestrutura.Orm.ModuloTarefa;
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
            var connectionString = builder.Configuration["SQL_CONNECTION_STRING"];

            return new SqlConnection(connectionString);
        });

        builder.Services.AddScoped<IRepositorioContato, RepositorioContatoEmOrm>();
        builder.Services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoEmOrm>();
        builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmOrm>();
        builder.Services.AddScoped<IRepositorioDespesa, RepositorioDespesaEmOrm>();
        builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefaEmOrm>();

        builder.Services.AddEntityFrameworkConfig(builder.Configuration);
        builder.Services.AddSerilogConfig(builder.Logging);

        builder.Services.AddControllersWithViews();

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