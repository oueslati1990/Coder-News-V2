using CoderNews.Api;
using CoderNews.Application;
using CoderNews.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
           .AddPresentation()
           .AddApplication()
           .AddInfrastructure(builder.Configuration);



var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
