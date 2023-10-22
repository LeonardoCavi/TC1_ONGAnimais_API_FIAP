using ONGAnimaisTelegramBot.Domain.Entities;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Interface
{
    public interface IBotManager
    {
        Task Iniciar(Atendimento atendimento);
        Task TratarRecebimento(Message mensagem, Atendimento atendimento);
        Task Notificar(string mensagem, Atendimento atendimento, bool encerrar);
    }
}
