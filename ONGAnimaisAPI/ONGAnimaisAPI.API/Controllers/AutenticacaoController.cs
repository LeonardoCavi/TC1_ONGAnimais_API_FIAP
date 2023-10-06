using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Application.ViewModels.Autorizacao;

namespace ONGAnimaisAPI.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoApplicationService _application;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;

        public AutenticacaoController(IAutenticacaoApplicationService application,
                                      IMapper mapper,
                                      INotificador notificador)
        {
            this._application = application;
            this._mapper = mapper;
            this._notificador = notificador;
        }

        [HttpPost]
        public async Task<IActionResult> Autenticao(AutenticaViewModel aut)
        {
            try
            {
                var token = await _application.Autenticar(aut);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", _mapper.Map<RespostaViewModel<string>>(token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }
    }
}
