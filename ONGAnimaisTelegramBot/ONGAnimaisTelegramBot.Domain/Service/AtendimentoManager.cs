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
        public AtendimentoManager(ILogger<AtendimentoManager> logger, IBotManager botManager)
        {
            _logger = logger;
            _botManager = botManager;
        }

        public async Task NovaMensagem(Message mensagem, Atendimento atendimento)
        {
            await _botManager.TratarRecebimento(atendimento, mensagem.Text);

            if (!atendimento.EmAtendimento)
                RemoverAtendimento(atendimento.SessaoId);
        }

        public async Task NovoAtendimento(Message mensagem, string sessaoId)
        {
            var atendimento = CriarAtendimento(mensagem, sessaoId);
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
    }
}
