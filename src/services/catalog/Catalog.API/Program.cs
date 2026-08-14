Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up the application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddCarter();
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    var connectionString = builder.Configuration.GetConnectionString("catalog-postgres-db")!;

    builder.Services.AddMarten(options =>
    {
        options.Connection(connectionString);

    }).UseLightweightSessions();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.InitializeMartenWith<CatalogInitialData>();
    }



    // 1. Add this where you configure your builder.Services
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddProblemDetails(); // Required for generating ProblemDetails responses



    builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "PostgreSQL Database");

    var app = builder.Build();

    // 2. Add this early in your middleware pipeline (before app.MapCarter())
    app.UseExceptionHandler(options => { });


    app.MapCarter();


    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


    await app.RunAsync();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly during startup");
}
finally
{
    Log.CloseAndFlush();
}
