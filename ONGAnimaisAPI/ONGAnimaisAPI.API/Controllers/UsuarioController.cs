using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
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
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
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
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuarioEventos);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
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
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuarioOngs);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
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
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());

                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", usuario);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        [Route("atualizar-usuario")]
        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(AtualizaUsuarioViewModel usuario)
        {
            try
            {
                await _application.AtualizarUsuario(usuario);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        [Route("excluir-usuario/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            try
            {
                await _application.ExcluirUsuario(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Usuário excluído com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
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

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Evento seguido com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        [Route("{usuarioId}/desseguir-evento/{id}")]
        [HttpPut]
        public async Task<IActionResult> DesseguirEvento(int usuarioId, int id)
        {
            try
            {
                await _application.DesseguirEvento(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Evento desseguido com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion

        #region [ONG]

        [Route("{usuarioId}/seguir-ong/{id}")]
        [HttpPut]
        public async Task<IActionResult> SeguirONG(int usuarioId, int id)
        {
            try
            {
                await _application.SeguirONG(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("ONG seguida com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        [Route("{usuarioId}/desseguir-ong/{id}")]
        [HttpPut]
        public async Task<IActionResult> DesseguirONG(int usuarioId, int id)
        {
            try
            {
                await _application.DesseguirONG(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("ONG desseguida com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion
    }
}
