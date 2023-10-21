using ONGAnimaisTelegramBot.Application.Service;
using ONGAnimaisTelegramBot.Domain.Interface;
using ONGAnimaisTelegramBot.Domain.Service;
using ONGAnimaisTelegramBot.Domain.Service.Bot;
using ONGAnimaisTelegramBot.Infra.Utility;
using ONGAnimaisTelegramBot.Infra.Vendors;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using ONGAnimaisTelegramBot.Worker.Handler;
using ONGAnimaisTelegramBot.Worker.TelegramClient;
using Polly;
using Telegram.Bot.Polling;

namespace ONGAnimaisTelegramBot.Worker.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotService, TelegramBotService>();
            services.AddSingleton<IUpdateHandler, UpdateHandler>();
            services.AddSingleton<IEventoService, EventoService>();
            services.AddSingleton<ISessaoMonitor, SessaoMonitor>();
            services.AddSingleton<IAtendimentoManager, AtendimentoManager>();
            services.AddSingleton<IBotManager, BotManager>();
            services.AddSingleton<IONGAPIHttpClient, ONGAPIHttpClient>();
            //services.AddSingleton<OngBot>();
            services.AddSingleton<HttpHelp>();
            services.AddSingleton<AsyncPolicy>(
               PollyConfiguration.CreateWaitAndRetryPolicy(new[]
               {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3)
               }));
        }
    }
}