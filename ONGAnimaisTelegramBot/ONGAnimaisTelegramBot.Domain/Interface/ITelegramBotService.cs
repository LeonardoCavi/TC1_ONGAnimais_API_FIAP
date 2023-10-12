using Telegram.Bot.Polling;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public interface ITelegramBotService
    {
        Task EscutarEventos(IUpdateHandler updateHandler, CancellationToken cancellationToken);
        Task EnviarMensagemAsync(string sessaoId, string texto);
        Task EnviarArquivoAsync( string sessaoId, byte[] buffer, string nomeArquivo);
        Task<byte[]> ObterArquivoAsync(string fileId);
    }
}