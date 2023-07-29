using Microsoft.Extensions.DependencyInjection;
using CoderNews.Infrastructure.Authentication;
using CoderNews.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;

namespace CoderNews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                 ConfigurationManager configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        return services;
    }
}