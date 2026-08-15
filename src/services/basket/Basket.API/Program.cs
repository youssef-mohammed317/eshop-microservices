using Discount.Grpc;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.Schema.For<ShoppingCart>().Identity(p => p.UserName);
}).UseLightweightSessions();


builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
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

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

var app = builder.Build();

app.UseExceptionHandler(options => { });

app.MapCarter();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


await app.RunAsync();
