using ONGAnimaisTelegramBot.Application.Service;
using ONGAnimaisTelegramBot.Domain.Interface;
using ONGAnimaisTelegramBot.Domain.Service;
using ONGAnimaisTelegramBot.Worker.Handler;
using ONGAnimaisTelegramBot.Worker.TelegramClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
