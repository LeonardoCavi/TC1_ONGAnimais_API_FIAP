namespace ONGAnimaisTelegramBot.Worker.Configurations
{
    public static class HttpClientConfiguration
    {
        public static void AddHttpClientConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient("HTTPOngAPI").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
        }
    }
}