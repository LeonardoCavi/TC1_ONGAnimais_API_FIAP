using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.Services;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using ONGAnimaisAPI.Domain.Services;
using ONGAnimaisAPI.Infra.Repositories;

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

            //Services
            services.AddScoped<IONGService, ONGService>();

            //Application Services
            services.AddScoped<IONGApplicationService, ONGApplicationService>();
        }
    }
}
