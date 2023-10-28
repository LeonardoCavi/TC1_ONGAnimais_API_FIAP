using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Domain.Interface;
using ONGAnimaisTelegramBot.Domain.Service.Bot;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public class BotManager : IBotManager
    {
        private string ClassName = typeof(BotManager).Name;
        private ILogger<BotManager> _logger;
        private ITelegramBotService _telegramBotService;
        private IONGAPIHttpClient _ongHttp;
        public BotManager(ILogger<BotManager> logger, ITelegramBotService telegramBotService, IONGAPIHttpClient ongHttp)
        {
            _logger = logger;
            _telegramBotService = telegramBotService;
            _ongHttp = ongHttp;
        }

        public async Task Iniciar(Atendimento atendimento)
        {
            var navegacaoBot = new OngBot(_telegramBotService, _ongHttp, atendimento);
            var result = await navegacaoBot.MenuCadastro();

            atendimento.MenuAnterior = result.Item2;
        }

        public async Task TratarRecebimento(Message mensagem, Atendimento atendimento)
        {
            await _telegramBotService.RemoverOpcoes(atendimento.SessaoId, atendimento.UltimaMensagemBot, mensagem.Text);
            var navegacaoBot = new OngBot(_telegramBotService, _ongHttp, atendimento);
            var resultado = await navegacaoBot.TratarResposta(atendimento.MenuAnterior, mensagem);
            
            atendimento.EmAtendimento = resultado.Item1;
            atendimento.MenuAnterior = resultado.Item2;
        }

        public async Task Notificar(string mensagem, Atendimento atendimento, bool encerrar)
        {
            var navegacaoBot = new OngBot(_telegramBotService, _ongHttp, atendimento);
            var resultado = await navegacaoBot.Notificar(mensagem, encerrar);

            if (!resultado.Item1)
            {
                await _telegramBotService.RemoverOpcoes(atendimento.SessaoId, atendimento.UltimaMensagemBot, string.Empty);
                atendimento.EmAtendimento = resultado.Item1;
            }
        }
    }
}
