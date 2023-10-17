using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ONGAnimaisTelegramBot.Domain.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Application.Service
{
    public class EventoService : IEventoService
    {
        private string ClassName = typeof(EventoService).Name;
        private readonly ILogger<EventoService> _logger;
        IAtendimentoManager _atendimentoManager;
        public EventoService(ILogger<EventoService> logger,
            IAtendimentoManager atendimentoManager)
        {
            _logger = logger;
            _atendimentoManager = atendimentoManager;
        }

        public async Task ReceberMensagem(Message mensagem, long? botId)
        {
            var intervalo = DateTime.Now.Subtract(mensagem.Date);

            if (mensagem != null && botId.HasValue && intervalo.TotalMinutes < 5)
                await ProcessarMensagem(mensagem, botId.Value);
            else
                _logger.LogWarning($"{ClassName}:ReceberMensagem => Evento ignorado: Objeto ou botId é nulo");
        }

        public async Task ReceberCallBack(CallbackQuery callback, long? botId)
        {
            var mensagem = new Message()
            {
                Text = callback.Data,
                Date = DateTime.Now,
                From = callback.From,
            };

            await ReceberMensagem(mensagem, botId);
        }

        private async Task ProcessarMensagem(Message mensagem, long? botId)
        {
            try
            {
                if (mensagem.From is null)
                {
                    _logger.LogInformation($"{ClassName}:ProcessarMensagem => Remetente está nulo, a mensagem será descartada");
                    return;
                }
                else
                {
                    string sessaoId = $"{mensagem.From.Id}:{botId}";

                    var atendimento = _atendimentoManager.ObterAtendimento(sessaoId);

                    if (atendimento is null)
                    {
                        _logger.LogInformation($"{ClassName}:ProcessarMensagem => Atendimento não foi encontrado. Iniciando sessão de atendimento para: {sessaoId}");
                        await _atendimentoManager.NovoAtendimento(mensagem, sessaoId);
                    }
                    else
                    {
                        _logger.LogInformation($"{ClassName}:ProcessarMensagem => Nova mensagem de: {sessaoId}");
                        await _atendimentoManager.NovaMensagem(mensagem, atendimento);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:ProcessarMensagem");
            }
        }
    }
}
