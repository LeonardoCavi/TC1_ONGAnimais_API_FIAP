using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;

namespace ONGAnimaisAPI.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class ONGController : ControllerBase
    {
        private readonly IONGApplicationService _application;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;

        public ONGController(IONGApplicationService application,
                             IMapper mapper,
                             INotificador notificador)
        {
            this._application = application;
            this._mapper = mapper;
            this._notificador = notificador;
        }

        #region [ONG]

        [Route("obter-ong/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterONG(int id)
        {
            try
            {
                var ong = await _application.ObterONG(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemONGViewModel>>(ong));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("obter-todas-ong")]
        [HttpGet]
        private async Task<IActionResult> ObterTodasONG()
        {
            try
            {
                var ongs = await _application.ObterTodasONG();

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemONGViewModel>>(ongs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("obter-ong-eventos/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterONGEventos(int id)
        {
            try
            {
                var ong = await _application.ObterONGEventos(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemONGEventosViewModel>>(ong));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("inserir-ong")]
        [HttpPost]
        public async Task<IActionResult> InserirONG(InsereONGViewModel ong)
        {
            try
            {
                await _application.InserirONG(ong);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", _mapper.Map<RespostaViewModel<InsereONGViewModel>>(ong));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("atualizar-ong")]
        [HttpPut]
        public async Task<IActionResult> AtualizarONG(AtualizaONGViewModel ong)
        {
            try
            {
                await _application.AtualizarONG(ong);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", _mapper.Map<RespostaViewModel<string>>("ONG atualizada com sucesso!"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("excluir-ong/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirONG(int id)
        {
            try
            {
                await _application.ExcluirONG(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        #endregion [ONG]

        #region [Evento]

        [Route("{ongId}/obter-evento/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterEvento(int ongId, int id)
        {
            try
            {
                var evento = await _application.ObterEvento(ongId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemEventoViewModel>>(evento));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("{ongId}/inserir-evento")]
        [HttpPost]
        public async Task<IActionResult> InserirEvento(int ongId, InsereEventoViewModel evento)
        {
            try
            {
                await _application.InserirEvento(ongId, evento);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", _mapper.Map<RespostaViewModel<InsereEventoViewModel>>(evento));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("{ongId}/atualizar-evento")]
        [HttpPut]
        public async Task<IActionResult> AtualizarEvento(int ongId, AtualizaEventoViewModel evento)
        {
            try
            {
                await _application.AtualizarEvento(ongId, evento);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", _mapper.Map<RespostaViewModel<string>>("Evento atualizado com sucesso!"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("{ongId}/excluir-evento/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirEvento(int ongId, int id)
        {
            try
            {
                await _application.ExcluirEvento(ongId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        #endregion [Evento]
    }
}