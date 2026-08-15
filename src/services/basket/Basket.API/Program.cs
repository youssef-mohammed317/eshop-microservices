var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var connectionString = builder.Configuration.GetConnectionString("basket-postgres-db")!;
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.Schema.For<ShoppingCart>().Identity(p => p.UserName);
}).UseLightweightSessions();


builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

var redisConnectionString = builder.Configuration.GetConnectionString("basket-redis-cache")!;
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    //options.InstanceName = "Basket";
});

// 1. Add this where you configure your builder.Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails(); // Required for generating ProblemDetails responses



builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "PostgreSQL Database")
    .AddRedis(redisConnectionString, name: "Redis Cache");
var app = builder.Build();

app.UseExceptionHandler(options => { });

app.MapCarter();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


await app.RunAsync();
