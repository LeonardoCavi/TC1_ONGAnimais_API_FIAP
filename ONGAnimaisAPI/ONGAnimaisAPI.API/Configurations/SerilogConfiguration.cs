using Serilog;

namespace ONGAnimaisAPI.API.Configurations
{
    public static class SerilogConfiguration
    {
        public static void AddSerilogConfiguration(this IServiceCollection services,
                                                   IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(configuration)
                .CreateLogger();
        }

        public static void UseSerilogConfiguration(this ConfigureHostBuilder configureHostBuilder)
        {
            configureHostBuilder.UseSerilog();
        }
    }
}