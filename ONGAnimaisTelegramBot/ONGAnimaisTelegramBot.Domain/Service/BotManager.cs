using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Domain.Interface;
using ONGAnimaisTelegramBot.Domain.Service.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public class BotManager : IBotManager
    {
        private string ClassName = typeof(BotManager).Name;
        private ILogger<BotManager> _logger;
        private OngBot _bot;
        public BotManager(ILogger<BotManager> logger, OngBot bot)
        {
            _logger = logger;
            _bot = bot;
        }

        public async Task Iniciar(Atendimento atendimento)
        {
            var result = await _bot.MenuPrincipal(atendimento.SessaoId);

            atendimento.MenuAnterior = result.Item2;
        }

        public async Task TratarRecebimento(Atendimento atendimento, string mensagem)
        {
            var resultado = await _bot.TratarResposta(atendimento.SessaoId, atendimento.MenuAnterior, mensagem);
            
            atendimento.EmAtendimento = resultado.Item1;
            atendimento.MenuAnterior = resultado.Item2;
        }
    }
}
