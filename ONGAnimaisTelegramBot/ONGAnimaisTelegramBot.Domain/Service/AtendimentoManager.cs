using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Domain.Interface;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public class AtendimentoManager : IAtendimentoManager
    {
        private string ClassName = typeof(AtendimentoManager).Name;
        private ILogger<AtendimentoManager> _logger;
        private ConcurrentDictionary<string, Atendimento> Atendimentos = new ConcurrentDictionary<string, Atendimento>();
        private IBotManager _botManager;
        private ISessaoMonitor _sessaoMonitor;
        public AtendimentoManager(ILogger<AtendimentoManager> logger, IBotManager botManager, ISessaoMonitor sessaoMonitor)
        {
            _logger = logger;
            _botManager = botManager;
            _sessaoMonitor = sessaoMonitor;

            _sessaoMonitor.SetNotificacaoPreDesconexao(NotificarPreDesconexao);
            _sessaoMonitor.SetNotificacaoDesconexao(NotificarDesconexao);
        }

        public async Task NovaMensagem(Message mensagem, Atendimento atendimento)
        {
            _sessaoMonitor.AtualizarSessao(atendimento.SessaoId);

            await _botManager.TratarRecebimento(mensagem, atendimento);

            if (!atendimento.EmAtendimento)
            {
                RemoverAtendimento(atendimento.SessaoId);
                _sessaoMonitor.RemoverSessao(atendimento.SessaoId);
            }
        }

        public async Task NovoAtendimento(Message mensagem, string sessaoId)
        {
            var atendimento = CriarAtendimento(mensagem, sessaoId);
            _sessaoMonitor.AdicionarSessao(sessaoId);
            await _botManager.Iniciar(atendimento);
        }

        public Atendimento ObterAtendimento(string sessaoId)
        {
            if (Atendimentos.ContainsKey(sessaoId))
                return Atendimentos[sessaoId];
            return null;
        }

        private Atendimento CriarAtendimento(Message mensagem, string sessaoId)
        {
            var nomeCliente = $"{mensagem.From.FirstName}{(string.IsNullOrEmpty(mensagem.From.LastName) ? "" : $" {mensagem.From.LastName}")}";
            var atendimento = new Atendimento()
            {
                SessaoId = sessaoId,
                NomeCliente = nomeCliente,
                InstanteUltimaMensagem = DateTime.Now,
                EmAtendimento = true
            };

            Atendimentos[sessaoId] = atendimento;

            return atendimento;
        }

        private void RemoverAtendimento(string sessaoId)
        {
            if (Atendimentos.ContainsKey(sessaoId))
                Atendimentos.TryRemove(sessaoId, out var _);
        }

        private async Task NotificarPreDesconexao(string sessaoId)
        {
            var atendimento = ObterAtendimento(sessaoId);
            await _botManager.Notificar("Atenção! Você será desconectado em breve, caso não interaja com o Bot.", atendimento, false);
        }

        private async Task NotificarDesconexao(string sessaoId)
        {
            var atendimento = ObterAtendimento(sessaoId);
            await _botManager.Notificar("Atendimento encerrado..", atendimento, true);
            RemoverAtendimento(sessaoId);
        }
    }
}
