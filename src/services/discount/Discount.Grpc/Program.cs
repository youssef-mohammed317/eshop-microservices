using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var connectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddDbContext<DiscountContext>(options =>
{
    options.UseSqlite(connectionString);
});


var app = builder.Build();

await app.UseMigrations();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();

app.Run();
