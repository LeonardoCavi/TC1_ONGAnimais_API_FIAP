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
        private readonly IONGAPIHttpClient _httpClient;

        public AtendimentoManager(ILogger<AtendimentoManager> logger,
                                  IONGAPIHttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public void NovaMensagem(Message mensagem, Atendimento atendimento)
        {
            //Direcionar atendimento para o Bot
            throw new NotImplementedException();
        }

        public void NovoAtendimento(Message mensagem, string sessaoId)
        {
            //Criar atendimento e acionar no dicionario de atendimentos
            //Iniciar atendimento no Bot em alguma service nova
            throw new NotImplementedException();
        }

        public Atendimento ObterAtendimento(string sessaoId)
        {
            if (Atendimentos.ContainsKey(sessaoId))
                return Atendimentos[sessaoId];
            return null;
        }
    }
}
