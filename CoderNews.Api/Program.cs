using CoderNews.Api.Errors;
using CoderNews.Application;
using CoderNews.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
           .AddApplication()
           .AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<ProblemDetailsFactory , CoderNewsProblemDetailsFactory>();


var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
