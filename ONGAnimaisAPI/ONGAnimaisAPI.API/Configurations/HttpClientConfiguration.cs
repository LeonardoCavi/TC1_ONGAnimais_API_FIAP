namespace ONGAnimaisAPI.API.Configurations
{
    public static class HttpClientConfiguration
    {
        public static void AddHttpClientConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient("HTTPGeocodingAPI").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
        }
    }
}
