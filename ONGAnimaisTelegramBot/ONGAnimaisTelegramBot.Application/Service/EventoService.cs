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
            if (mensagem != null && botId.HasValue)
                await ProcessarMensagem(mensagem, botId.Value);
            else
                _logger.LogWarning($"{ClassName}:ReceberMensagem => Evento ignorado: Objeto ou botId é nulo");
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

                    // Verificar se o cliente já está em atendimento
                    var atendimento = _atendimentoManager.ObterAtendimento(sessaoId);

                    if (atendimento is null)
                    {
                        _logger.LogInformation($"{ClassName}:ProcessarMensagem => Atendimento não foi encontrado. Iniciando sessão de atendimento para: {sessaoId}");
                        _atendimentoManager.NovoAtendimento(mensagem, sessaoId);
                    }
                    else
                    {
                        _logger.LogInformation($"{ClassName}:ProcessarMensagem => Nova mensagem de: {sessaoId}");
                        _atendimentoManager.NovaMensagem(mensagem, atendimento);
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
