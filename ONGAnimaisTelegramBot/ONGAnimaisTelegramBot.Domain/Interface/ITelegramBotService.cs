using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public interface ITelegramBotService
    {
        Task EscutarEventos(IUpdateHandler updateHandler, CancellationToken cancellationToken);
        Task<Message> EnviarMensagem(string sessaoId, string texto);
        Task<Message> EnviarMensagem(string sessaoId, string texto, IDictionary<string, string> opcoes);
        Task<Message> EnviarPedidoLocalizacao(string sessaoId, string texto);
        Task<Message> EnviarArquivo( string sessaoId, byte[] buffer, string nomeArquivo);
        Task<byte[]> ObterArquivo(string fileId);
        Task RemoverOpcoes(string sessaoId, Message mensagem, string texto);
    }
}