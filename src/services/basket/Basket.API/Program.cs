using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// ==========================================
// 1. Application Layer Services
// ==========================================
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// ==========================================
// 2. Data Access & Caching Layer (Infrastructure)
// ==========================================
var connectionString = builder.Configuration.GetConnectionString("Database")!;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

// Marten (PostgreSQL) Configuration
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.Schema.For<ShoppingCart>().Identity(p => p.UserName);
}).UseLightweightSessions();

// Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    //options.InstanceName = "Basket";
});

// Repositories Registration & Decorators
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// ==========================================
// 3. External Services (gRPC Clients)
// ==========================================
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

builder.Services.AddMessageBroker(builder.Configuration);

// ==========================================
// 4. Cross-Cutting Concerns
// ==========================================
// Exception Handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails(); // Required for generating ProblemDetails responses

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "PostgreSQL Database")
    .AddRedis(redisConnectionString, name: "Redis Cache");

// ==========================================
// 5. Build and HTTP Request Pipeline
// ==========================================
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(options => { });

app.MapCarter();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();