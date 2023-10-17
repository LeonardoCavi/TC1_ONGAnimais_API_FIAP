using ONGAnimaisTelegramBot.Worker;
using ONGAnimaisTelegramBot.Worker.Configurations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSerilogConfiguration(hostContext.Configuration);
        services.AddDependencyInjection();
        services.AddHttpClientConfiguration();
        services.AddVendorConfiguration(hostContext.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
