using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Application.ViewModels.Autorizacao;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ONGAnimaisAPI.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoApplicationService _application;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;
        private readonly ILogger<AutenticacaoController> _logger;
        private readonly string _className = typeof(AutenticacaoController).Name;

        public AutenticacaoController(IAutenticacaoApplicationService application,
                                      IMapper mapper,
                                      INotificador notificador,
                                      ILogger<AutenticacaoController> logger)
        {
            this._application = application;
            this._mapper = mapper;
            this._notificador = notificador;
            this._logger = logger;
        }

        /// <summary>
        /// Autenticação por usuário e senha para geração de token de acesso
        /// </summary>
        /// <param name="aut"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Autenticacao(AutenticaViewModel aut)
        {
            try
            {

                _logger.LogInformation($"[{_className}] - [Autenticacao] => Request.: {JsonSerializer.Serialize(aut)}");
                
                var token = await _application.Autenticar(aut);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());

                    _logger.LogWarning($"[{_className}] - [Autenticacao] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", new {Token = token});
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [Autenticacao] => Exception.: {ex.Message}");
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }
    }
}
