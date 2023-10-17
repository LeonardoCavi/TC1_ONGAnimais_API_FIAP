using ONGAnimaisTelegramBot.Infra.Vendors.Config;

namespace ONGAnimaisTelegramBot.Worker.Configurations
{
    public static class VendorsConfiguration
    {
        public static void AddVendorConfiguration(this IServiceCollection services,
                                                  IConfiguration configuration)
        {
            var ongAPIConfig = configuration.GetSection("ONGApi").Get<ONGApiConfig>();
            if(ongAPIConfig != null )
            {
                services.AddSingleton(ongAPIConfig);
            }
        }
    }
}