using Microsoft.Extensions.DependencyInjection;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.Services;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Repositories;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using ONGAnimaisAPI.Domain.Interfaces.Utility;
using ONGAnimaisAPI.Domain.Notifications;
using ONGAnimaisAPI.Domain.Services;
using ONGAnimaisAPI.Infra.Repositories;
using ONGAnimaisAPI.Infra.Utility;
using ONGAnimaisAPI.Infra.Utility.Geocoding;
using Polly;

namespace ONGAnimaisAPI.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped<IONGRepository, ONGRepository>();
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAplicacaoRepository, AplicacaoRepository>();

            //Services
            services.AddScoped<IONGService, ONGService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAplicacaoService, AplicacaoService>();
            services.AddScoped<ITokenService, TokenService>();

            //Application Services
            services.AddScoped<IONGApplicationService, ONGApplicationService>();
            services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();
            services.AddScoped<IAutenticacaoApplicationService, AutenticacaoApplicationService>();

            //Notificador
            services.AddScoped<INotificador, Notificador>();

            //Utility HttpClient
            services.AddScoped<IGeocodingAPIHttpClient, GeocodingAPIHttpClient>();
            services.AddScoped<HttpHelp>();
            services.AddSingleton<AsyncPolicy>(
               PollyConfiguration.CreateWaitAndRetryPolicy(new[]
               {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3)
               }));
        }
    }
}
