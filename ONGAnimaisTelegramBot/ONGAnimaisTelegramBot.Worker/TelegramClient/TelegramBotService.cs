using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ONGAnimaisTelegramBot.Domain.Service;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ONGAnimaisTelegramBot.Worker.TelegramClient
{
    public class TelegramBotService : ITelegramBotService
    {
        private string ClassName = typeof(TelegramBotService).Name;
        private ILogger<TelegramBotService> _logger;
        private IConfiguration _configuration;
        private TelegramBotClient telegramBotClient;
        public TelegramBotService(
            ILogger<TelegramBotService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            ConfigurarClient();
        }

        private void ConfigurarClient()
        {
            var botToken = _configuration.GetValue<string>("BotToken");

            telegramBotClient = new TelegramBotClient(botToken);

            var me = telegramBotClient.GetMeAsync().Result;

            _logger.LogInformation($"{ClassName}:ConfigurarClient => Me: {JsonConvert.SerializeObject(me)}");
        }

        public async Task EscutarEventos(IUpdateHandler updateHandler, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                ReceiverOptions options = new()
                {
                    AllowedUpdates = new UpdateType[] { UpdateType.Message, UpdateType.CallbackQuery }
                };

                await telegramBotClient.ReceiveAsync(
                    updateHandler: updateHandler,
                    receiverOptions: options,
                    cancellationToken: cancellationToken);

            }, cancellationToken);

            await Task.CompletedTask;
        }

        public async Task EnviarMensagem(string sessaoId, string texto)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                var message = await telegramBotClient.SendTextMessageAsync(
                    chatId: clientId,
                    text: texto);

                _logger.LogInformation($"{ClassName}:EnviarMensagemAsync => ClientId: {clientId}; Texto: {texto}; MessageId: {message.MessageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarMensagemAsync => {ex.Message}");
            }
        }

        public async Task EnviarMensagem(string sessaoId, string texto, IDictionary<string, string> opcoes)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                var buttons = opcoes.Select(x => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(x.Value, x.Key) });
                var keyboardMarkup = new InlineKeyboardMarkup(buttons);

                var message = await telegramBotClient.SendTextMessageAsync(
                    chatId: clientId,
                    text: texto,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: keyboardMarkup);

                _logger.LogInformation($"{ClassName}:EnviarMensagemAsync => ClientId: {clientId}; Texto: {texto}; MessageId: {message.MessageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarMensagemAsync => {ex.Message}");
            }
        }

        public async Task EnviarArquivo(string sessaoId, byte[] buffer, string nomeArquivo)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                var stream = new MemoryStream(buffer);

                var message = await telegramBotClient.SendDocumentAsync(
                    chatId: clientId,
                    document: InputFile.FromStream(stream, nomeArquivo));

                _logger.LogInformation($"{ClassName}:EnviarArquivoAsync => ClientId: {clientId}; NomeArquivo: {nomeArquivo}; MessageId {message.MessageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarArquivoAsync => {ex.Message}");
            }
        }

        public async Task<byte[]> ObterArquivo(string fileId)
        {
            try
            {
                using var stream = new MemoryStream();
                var file = await telegramBotClient.GetInfoAndDownloadFileAsync(fileId, stream);

                if (file is null)
                    return null;

                return stream.GetBuffer();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:ObterArquivoAsync => {ex.Message}");
                return null;
            }
        }

        private long ObterClientId(string sessaoId)
        {
            var clientId = long.Parse(sessaoId.Split(':')[0]);

            _logger.LogInformation($"{ClassName}:ObterClientId => Obtendo '{clientId}' de {sessaoId}");

            return clientId;
        }
    }
}
