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

        public async Task<Message> EnviarMensagem(string sessaoId, string texto)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                var message = await telegramBotClient.SendTextMessageAsync(
                    chatId: clientId,
                    text: texto,
                    replyMarkup: new ReplyKeyboardRemove());

                _logger.LogInformation($"{ClassName}:EnviarMensagemAsync => ClientId: {clientId}; Texto: {texto}; MessageId: {message.MessageId}");
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarMensagemAsync => {ex.Message}");
                return null;
            }
        }

        public async Task<Message> EnviarMensagem(string sessaoId, string texto, IDictionary<string, string> opcoes)
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
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarMensagemAsync => {ex.Message}");
                return null;
            }
        }
        public async Task<Message> EnviarPedidoLocalizacao(string sessaoId, string texto)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{ KeyboardButton.WithRequestLocation("Minha localização")})
                { 
                    OneTimeKeyboard = true,
                    ResizeKeyboard = true
                };

                var message = await telegramBotClient.SendTextMessageAsync(
                    chatId: clientId,
                    text: texto,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: replyKeyboardMarkup);

                _logger.LogInformation($"{ClassName}:EnviarPedidoLocalizacao => ClientId: {clientId}; Texto: {texto}; MessageId: {message.MessageId}");
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarPedidoLocalizacao => {ex.Message}");
                return null;
            }
        }

        public async Task<Message> EnviarArquivo(string sessaoId, byte[] buffer, string nomeArquivo)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                var stream = new MemoryStream(buffer);

                var message = await telegramBotClient.SendDocumentAsync(
                    chatId: clientId,
                    document: InputFile.FromStream(stream, nomeArquivo));

                _logger.LogInformation($"{ClassName}:EnviarArquivoAsync => ClientId: {clientId}; NomeArquivo: {nomeArquivo}; MessageId {message.MessageId}");
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:EnviarArquivoAsync => {ex.Message}");
                return null;
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

        public async Task RemoverOpcoes(string sessaoId, Message mensagem, string texto)
        {
            try
            {
                var clientId = ObterClientId(sessaoId);

                if (mensagem.ReplyMarkup != null)
                {
                    var mensagemEditada = mensagem.Text;

                    var textoOpcoes = new List<string>();
                    var negritouOpcao = false;
                    foreach (var row in mensagem.ReplyMarkup.InlineKeyboard)
                    {
                        foreach (var button in row)
                        {
                            if (button.CallbackData.Contains(texto.ToLower()) && !negritouOpcao  && !string.IsNullOrEmpty(texto))
                            {
                                textoOpcoes.Add($"*{button.Text} ✅*");
                                negritouOpcao = true;
                            }
                            else
                                textoOpcoes.Add($"{button.Text}");
                        }
                    }

                    var opcoes = string.Join(Environment.NewLine, textoOpcoes);
                    mensagemEditada += $"{Environment.NewLine}{Environment.NewLine}{opcoes}";
                    await telegramBotClient.EditMessageTextAsync(clientId, mensagem.MessageId, mensagemEditada, ParseMode.Markdown);
                }
            }
            catch
            {

            }
        }
    }
}
