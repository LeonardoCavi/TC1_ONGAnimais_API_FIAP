using ONGAnimaisTelegramBot.Worker;
using ONGAnimaisTelegramBot.Worker.Configurations;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSerilogConfiguration(hostContext.Configuration);
        services.AddDependencyInjection();
        services.AddHttpClientConfiguration();
        services.AddVendorConfiguration(hostContext.Configuration);
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

host.Run();
