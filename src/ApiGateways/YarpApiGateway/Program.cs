using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    // Global options (optional)
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Return 429 when limit exceeded

    // Define a specific policy named "fixed-policy"
    options.AddFixedWindowLimiter(policyName: "fixed-policy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// 2. Add Rate Limiter Middleware to the pipeline
// (Must be added BEFORE MapReverseProxy)
app.UseRateLimiter();

app.MapReverseProxy();

app.Run();