using Telegram.Bot.Polling;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public interface ITelegramBotService
    {
        Task EscutarEventos(IUpdateHandler updateHandler, CancellationToken cancellationToken);
        Task EnviarMensagem(string sessaoId, string texto);
        Task EnviarMensagem(string sessaoId, string texto, IDictionary<string, string> opcoes);
        Task EnviarArquivo( string sessaoId, byte[] buffer, string nomeArquivo);
        Task<byte[]> ObterArquivo(string fileId);
    }
}