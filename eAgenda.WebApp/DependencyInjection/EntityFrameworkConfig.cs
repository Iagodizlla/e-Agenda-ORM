using eAgenda.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.DependencyInjection;

public static class EntityFrameworkConfig
{
    public static void AddEntityFrameworkConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["SQL_CONNECTION_STRING"];

        services.AddDbContext<eAgendaDbContext>(options => options.UseSqlServer(connectionString));
    }
}