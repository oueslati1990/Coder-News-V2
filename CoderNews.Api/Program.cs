using AspNetCoreRateLimit;
using CoderNews.Api;
using CoderNews.Application;
using CoderNews.Infrastructure;
using Serilog;

// Configure Serilog early for startup logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting CoderNews API");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console()
        .WriteTo.Seq(
            serverUrl: context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5342",
            apiKey: context.Configuration["Seq:ApiKey"]));

    // Configure rate limiting
    builder.Services.AddMemoryCache();
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    builder.Services
               .AddPresentation()
               .AddApplication()
               .AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    // Configure Swagger for Development mode only
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoderNews API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "CoderNews API Documentation";
            options.DisplayRequestDuration();
        });
    }

    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();

    // Apply rate limiting
    app.UseIpRateLimiting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("CoderNews API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down CoderNews API");
    Log.CloseAndFlush();
}
