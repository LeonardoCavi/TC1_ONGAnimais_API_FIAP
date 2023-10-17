using ONGAnimaisTelegramBot.Domain.Entities;

namespace ONGAnimaisTelegramBot.Domain.Interface
{
    public interface IBotManager
    {
        Task Iniciar(Atendimento atendimento);
        Task TratarRecebimento(Atendimento atendimento, string mensagem);
    }
}
