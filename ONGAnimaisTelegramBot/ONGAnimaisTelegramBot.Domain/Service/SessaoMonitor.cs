using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Domain.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Service
{
    public class SessaoMonitor : ISessaoMonitor
    {
        private string ClassName = typeof(SessaoMonitor).Name;
        private ILogger<SessaoMonitor> _logger;
        private ConcurrentDictionary<string, Sessao> Sessoes = new ConcurrentDictionary<string, Sessao>();
        private Func<string, Task> NotificarPreDesconexao;
        private Func<string, Task> NotificarDesconexao;
        private int limiteOciosidadeMinutos;
        public SessaoMonitor(ILogger<SessaoMonitor> logger, IConfiguration configuration)
        {
            _logger = logger;
            limiteOciosidadeMinutos = configuration.GetValue<int>("LimiteOciosidadeMinutos");
        }

        public async Task IniciarMonitoramento(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{ClassName}:IniciarMonitoramento => Iniciando processo de monitoramento de sessões ociosas");

            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"{ClassName}:IniciarMonitoramento => Total de sessões em atendimento: {Sessoes.Count}");

                    await MonitorarSessoesPreOciosas();

                    await MonitorarSessoesOciosas();

                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }, cancellationToken);
        }

        private async Task MonitorarSessoesPreOciosas()
        {
            try
            {
                var agoraMinutosTotais = DateTime.Now.TimeOfDay.TotalMinutes;
                var sessoesPreOciosas = Sessoes.Values.Where(x => (agoraMinutosTotais - x.InstanteUltimaMensagem.TimeOfDay.TotalMinutes > limiteOciosidadeMinutos - 1) && !x.NotificacaoPreOciosidadeEnviada);

                if (sessoesPreOciosas.Any())
                {
                    _logger.LogInformation($"{ClassName}:MonitorarSessoesPreOciosas => Total de sessões em pré desconexão: {sessoesPreOciosas.Count()}");

                    foreach (var sessaoPreOciosa in sessoesPreOciosas)
                    {
                        _logger.LogInformation($"{ClassName}:MonitorarSessoesPreOciosas => Sessão '{sessaoPreOciosa.SessaoId}' será notificada");

                        sessaoPreOciosa.NotificacaoPreOciosidadeEnviada = true;
                        await NotificarPreDesconexao(sessaoPreOciosa.SessaoId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:MonitorarSessoesPreOciosas");
            }
        }

        private async Task MonitorarSessoesOciosas()
        {
            try
            {
                var agoraMinutosTotais = DateTime.Now.TimeOfDay.TotalMinutes;
                var sessoesOciosas = Sessoes.Values.Where(x => agoraMinutosTotais - x.InstanteUltimaMensagem.TimeOfDay.TotalMinutes > limiteOciosidadeMinutos);

                if (sessoesOciosas.Any())
                {
                    _logger.LogInformation($"{ClassName}:MonitorarSessoesOciosas => Total de sessões: {sessoesOciosas.Count()}");

                    foreach (var sessaoOciosa in sessoesOciosas)
                    {
                        _logger.LogInformation($"{ClassName}:MonitorarSessoesOciosas => Sessão '{sessaoOciosa.SessaoId}' será encerrada");

                        RemoverSessao(sessaoOciosa.SessaoId);
                        await NotificarDesconexao(sessaoOciosa.SessaoId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ClassName}:MonitorarSessoesOciosas");
            }
        }

        public void SetNotificacaoPreDesconexao(Func<string, Task> action)
        {
            NotificarPreDesconexao = action;
        }

        public void SetNotificacaoDesconexao(Func<string, Task> action)
        {
            NotificarDesconexao = action;
        }

        public void AdicionarSessao(string sessaoId)
        {
            var sessao = new Sessao()
            {
                SessaoId = sessaoId,
                InstanteUltimaMensagem = DateTime.Now,
                NotificacaoPreOciosidadeEnviada = false
            };

            Sessoes[sessao.SessaoId] = sessao;
        }

        public void AtualizarSessao(string sessaoId)
        {
            if (Sessoes.ContainsKey(sessaoId))
            {
                var sessao = Sessoes[sessaoId];
                sessao.InstanteUltimaMensagem = DateTime.Now;
                sessao.NotificacaoPreOciosidadeEnviada = false;
            }
        }

        public void RemoverSessao(string sessaoId)
        {
            if (Sessoes.ContainsKey(sessaoId))
                Sessoes.TryRemove(sessaoId, out _);
        }
    }
}
