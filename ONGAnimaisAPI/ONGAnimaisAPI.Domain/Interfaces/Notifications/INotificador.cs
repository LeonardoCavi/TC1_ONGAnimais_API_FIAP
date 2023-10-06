using ONGAnimaisAPI.Domain.Notifications;

namespace ONGAnimaisAPI.Domain.Interfaces.Notifications
{
    public interface INotificador
    {
        IReadOnlyCollection<Notificacao> ObterNotificacoes();
        bool TemNotificacao();
        void AddNotificacao(Notificacao notificacao);
    }
}