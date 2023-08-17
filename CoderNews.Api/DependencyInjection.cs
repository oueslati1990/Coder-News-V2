using CoderNews.Api.Common.Errors;
using CoderNews.Api.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CoderNews.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, CoderNewsProblemDetailsFactory>();
        services.AddMappings();
        
        return services;
    }
}