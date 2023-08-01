using Microsoft.Extensions.DependencyInjection;
using CoderNews.Infrastructure.Authentication;
using CoderNews.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using CoderNews.Application.Common.Interfaces.Persistence;
using CoderNews.Infrastructure.Persistence;

namespace CoderNews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                 ConfigurationManager configuration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUserRepository , UserRepository>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        return services;
    }
}