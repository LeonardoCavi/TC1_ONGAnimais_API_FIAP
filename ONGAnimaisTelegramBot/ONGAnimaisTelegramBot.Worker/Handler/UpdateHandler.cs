using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using ONGAnimaisTelegramBot.Domain.Interface;

namespace ONGAnimaisTelegramBot.Worker.Handler
{
    internal class UpdateHandler : IUpdateHandler
    {
        private string ClassName = typeof(UpdateHandler).Name;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly IEventoService _eventoService;

        public UpdateHandler(ILogger<UpdateHandler> logger, IEventoService eventoService)
        {
            _logger = logger;
            _eventoService = eventoService;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{ClassName}:HandleUpdateAsync => Novo evento: {JsonConvert.SerializeObject(update)}");

            switch (update.Type)
            {
                case UpdateType.Message:
                    await _eventoService.ReceberMensagem(update.Message, botClient.BotId);
                    break;
                default:
                    _logger.LogInformation($"{ClassName}:HandleUpdateAsync => Evento ignorado: {JsonConvert.SerializeObject(update.Type)}");
                    break;
            }
        }
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"{ClassName}:PollingErrorHandler => Houve uma exceção durante a obtenção de eventos: {exception.Message}");
        }
    }
}