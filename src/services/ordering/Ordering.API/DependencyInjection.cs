using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ordering.API.Exceptions;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")!;

        services.AddCarter();
        services.AddExceptionHandler<OrderingExceptionHandler>();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: connectionString,
                            name: "SqlServer",
                            tags: new[] { "db", "sql", "sqlserver" });
        return services;
    }
    public static IApplicationBuilder UseApiServices(this WebApplication app)
    {
        app.UseExceptionHandler(options => { });

        app.MapCarter();

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
