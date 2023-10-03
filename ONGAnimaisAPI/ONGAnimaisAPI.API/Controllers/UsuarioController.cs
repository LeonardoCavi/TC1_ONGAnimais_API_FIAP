using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Notifications;
using System.Security.Cryptography;

namespace ONGAnimaisAPI.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplicationService _application;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;
        public UsuarioController(IUsuarioApplicationService application, IMapper mapper, INotificador notificador)
        {
            this._application = application;
            this._mapper = mapper;
            this._notificador = notificador;
        }

        #region [Usuario]

        [Route("obter-usuario/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            try
            {
                var usuario = await _application.ObterUsuario(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemUsuarioViewModel>>(usuario));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("obter-todos-usuarios")]
        [HttpGet]
        private async Task<IActionResult> ObterTodosUsuarios()
        {
            try
            {
                var usuarios = await _application.ObterTodosUsuarios();

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<List<ObtemUsuarioViewModel>>>(usuarios));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("obter-usuario-eventos/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuarioEventos(int id)
        {
            try
            {
                var usuarioEventos = await _application.ObterUsuarioEventos(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemUsuarioEventosViewModel>>(usuarioEventos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("obter-usuario-ongs/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuarioONGs(int id)
        {
            try
            {
                var usuarioOngs = await _application.ObterUsuarioONGs(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(_mapper.Map<RespostaViewModel<ObtemUsuarioONGsViewModel>>(usuarioOngs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("inserir-usuario")]
        [HttpPost]
        public async Task<IActionResult> InserirUsuario(InsereUsuarioViewModel usuario)
        {
            try
            {
                await _application.InserirUsuario(usuario);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<RespostaViewModel<object>>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", _mapper.Map<RespostaViewModel<InsereUsuarioViewModel>>(usuario));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("atualizar-usuario")]
        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(AtualizaUsuarioViewModel usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return BadRequest("Dados do Usuário incorretos!");
                }
                if (usuario.Id <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }


                await _application.AtualizarUsuario(usuario);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("excluir-usuario/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }

                await _application.ExcluirUsuario(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        #endregion

        #region [Evento]

        [Route("{usuarioId}/seguir-evento/{id}")]
        [HttpPut]
        public async Task<IActionResult> SeguirEvento(int usuarioId, int id)
        {
            try
            {
                await _application.SeguirEvento(usuarioId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("{usuarioId}/desseguir-evento/{id}")]
        [HttpPut]
        public async Task<IActionResult> DesseguirEvento(int usuarioId, int id)
        {
            try
            {
                await _application.DesseguirEvento(usuarioId, id);


                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        #endregion

        #region [ONG]

        [Route("{usuarioId}/seguir-ong")]
        [HttpPost]
        public async Task<IActionResult> SeguirONG(int ongId, int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }

                if (ongId <= 0)
                {
                    return BadRequest("Identificador da ONG inválido. Tente novamente!");
                }

                await _application.SeguirONG(ongId, usuarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("{usuarioId}/desseguir-ong")]
        [HttpDelete]
        public async Task<IActionResult> DesseguirONG(int ongId, int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }

                if (ongId <= 0)
                {
                    return BadRequest("Identificador da ONG inválido. Tente novamente!");
                }

                await _application.DesseguirONG(ongId, usuarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        #endregion
    }
}
