using ONGAnimaisTelegramBot.Domain.Interface;
using ONGAnimaisTelegramBot.Domain.Service;
using Telegram.Bot.Polling;

namespace ONGAnimaisTelegramBot.Worker
{
    public class Worker : BackgroundService
    {
        private readonly string ClassName = typeof(Worker).Name;
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ClassName}:ExecuteAsync: Aplicação iniciada..");

            while (!stoppingToken.IsCancellationRequested)
            {
                _ = Task.Run(async () =>
                {
                    using var scope = _serviceProvider.CreateScope();

                    var telegramBotService = scope.ServiceProvider.GetRequiredService<ITelegramBotService>();
                    var updateHandler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
                    var sessaoMonitor = scope.ServiceProvider.GetRequiredService<ISessaoMonitor>();
                    await telegramBotService.EscutarEventos(updateHandler, stoppingToken);
                    await sessaoMonitor.IniciarMonitoramento(stoppingToken);
                });

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}