using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Infra;

namespace ONGAnimaisAPI.API.Configurations
{
    public static class DBContextConfiguration
    {
        public static void AddDBContextConfiguration(this IServiceCollection services,
                                                     IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ApplicationConnectionString"));
            });
        }

        public static void Seed(this IApplicationBuilder app,
                                IConfiguration configuration)
        {
            using var serviceScoped = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScoped.ServiceProvider
                .GetService<ApplicationDbContext>();

            context.Database.Migrate();

            if (context.Aplicacoes.Any())
            {
                return;
            }

            var usuario = configuration.GetSection("APICredencials:User").Value;
            usuario = string.IsNullOrEmpty(usuario)? "admin" : usuario;

            var senha = configuration.GetSection("APICredencials:Password").Value;
            senha = string.IsNullOrEmpty(senha) ? "admin" : senha;

            var usuarioDb = new Aplicacao
            {
                NomeAplicacao = "Usuário administrador da API",
                Usuario = usuario,
                Senha = senha
            };

            context.Add(usuarioDb);
            context.SaveChanges();
        }
    }
}