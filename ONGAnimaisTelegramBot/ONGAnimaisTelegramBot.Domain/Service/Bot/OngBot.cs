using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Service.Bot
{
    public class OngBot
    {
        private string ClassName = typeof(OngBot).Name;
        private ILogger<OngBot> _logger;
        private ITelegramBotService _telegramBotService;

        public OngBot(ILogger<OngBot> logger, ITelegramBotService telegramBotService)
        {
            _logger = logger;
            _telegramBotService = telegramBotService;
        }

        public async Task<Tuple<bool,string>> TratarResposta(string sessaoId, string menuAnterior, string mensagem)
        {
            if (menuAnterior == "MenuPrincipal")
            {
                if (mensagem == "1" || mensagem.ToLower() == "evento")
                {
                    return await MenuEvento(sessaoId);
                }
                else if(mensagem == "2" || mensagem.ToLower() == "ong")
                {
                    return await MenuONG(sessaoId);
                }
                else if (mensagem == "3" || mensagem.ToLower() == "usuario")
                {
                    return await MenuUsuario(sessaoId);
                }
                return await MenuPrincipal(sessaoId);
            }

            else
            {
                return Tuple.Create(false, string.Empty);
            }
        }

        public async Task<Tuple<bool, string>> MenuPrincipal(string sessaoId)
        {
            var mensagem = "Olá, seja bem-vindo ao Bot. Selecione uma das opções abaixo:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuPrincipal");
        }

        private async Task<Tuple<bool, string>> MenuEvento(string sessaoId)
        {
            var mensagem = "Selecione uma das opções:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuEvento");
        }

        private async Task<Tuple<bool, string>> MenuONG(string sessaoId)
        {
            var mensagem = "Selecione uma das opc:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuONG");
        }

        private async Task<Tuple<bool, string>> MenuUsuario(string sessaoId)
        {
            var mensagem = "Selecione uma das opc:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuUsuario");
        }
    }
}
