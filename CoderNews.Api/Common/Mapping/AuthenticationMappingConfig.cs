using CoderNews.Application.Authentication.Common;
using CoderNews.Application.Authentication.Queries.Login;
using CoderNews.Authentication.Commands.Register;
using CoderNews.Contracts.Authentication;
using Mapster;

namespace CoderNews.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult , AuthenticationResponse>()
              .Map(dest => dest.Token , src => src.Token)
              .Map(dest => dest , src => src.User);
    }
}
