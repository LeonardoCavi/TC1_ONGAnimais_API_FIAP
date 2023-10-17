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
using Telegram.Bot.Types.ReplyMarkups;

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
                case UpdateType.CallbackQuery:

                    await RemoverOpcoes(botClient, update.CallbackQuery);

                    await _eventoService.ReceberCallBack(update.CallbackQuery, botClient.BotId);
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

        private async Task RemoverOpcoes(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            try
            {
                if (callbackQuery.Message.ReplyMarkup != null)
                {
                    var mensagemEditada = callbackQuery.Message.Text;

                    var textoOpcoes = new List<string>();

                    foreach (var row in callbackQuery.Message.ReplyMarkup.InlineKeyboard)
                    {
                        foreach (var button in row)
                        {
                            if (button.CallbackData == callbackQuery.Data)
                                textoOpcoes.Add($"*{button.Text} ✅*");
                            else
                                textoOpcoes.Add($"{button.Text}");
                        }
                    }

                    var opcoes = string.Join(Environment.NewLine, textoOpcoes);
                    mensagemEditada += $"{Environment.NewLine}{Environment.NewLine}{opcoes}";
                    await botClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, mensagemEditada, ParseMode.Markdown);
                }
            }
            catch
            {

            }
        }
    }
}