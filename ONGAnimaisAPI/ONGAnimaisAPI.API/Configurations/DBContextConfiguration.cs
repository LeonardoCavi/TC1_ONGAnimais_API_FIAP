using Microsoft.EntityFrameworkCore;
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
    }
}
