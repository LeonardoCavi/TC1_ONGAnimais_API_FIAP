using ONGAnimaisTelegramBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Interface
{
    public interface ISessaoMonitor
    {
        Task IniciarMonitoramento(CancellationToken cancellationToken);
        void SetNotificacaoPreDesconexao(Func<string, Task> action);
        void SetNotificacaoDesconexao(Func<string, Task> action);
        void AdicionarSessao(string sessaoId);
        void AtualizarSessao(string sessaoId);
        void RemoverSessao(string sessaoId);
    }
}
