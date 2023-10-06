using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Domain.Notifications
{
    public class Notificador : INotificador
    {
        private readonly List<Notificacao> _notificacoes = new List<Notificacao>();

        public void AddNotificacao(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public IReadOnlyCollection<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
