using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using System.Security.Cryptography;
using System.Text.Json;

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
        private readonly ILogger<UsuarioController> _logger;
        private readonly string _className = typeof(UsuarioController).Name;

        public UsuarioController(IUsuarioApplicationService application,
                                 IMapper mapper,
                                 INotificador notificador,
                                 ILogger<UsuarioController> logger)
        {
            this._application = application;
            this._mapper = mapper;
            this._notificador = notificador;
            this._logger = logger;
        }

        #region [Usuario]

        /// <summary>
        /// Obter/Consultar Usuário pelo identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("obter-usuario/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterUsuario] => Request.: {new { Id = id }}");

                var usuario = await _application.ObterUsuario(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterUsuario] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterUsuario] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar todos Usuários
        /// </summary>
        /// <returns></returns>
        [Route("obter-todos-usuarios")]
        [HttpGet]
        private async Task<IActionResult> ObterTodosUsuarios()
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterTodosUsuarios] => Request.: n/a");
                var usuarios = await _application.ObterTodosUsuarios();

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterTodosUsuarios] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterTodosUsuarios] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar Usuário e seus Eventos seguidos pelo identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("obter-usuario-eventos/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuarioEventos(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterUsuarioEventos] => Request.: {new { Id = id }}");

                var usuarioEventos = await _application.ObterUsuarioEventos(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterUsuarioEventos] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuarioEventos);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterUsuarioEventos] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar Usuário e suas ONGs seguidas pelo identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("obter-usuario-ongs/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuarioONGs(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterUsuarioONGs] => Request.: {new { Id = id }}");

                var usuarioOngs = await _application.ObterUsuarioONGs(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterUsuarioONGs] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuarioOngs);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterUsuarioONGs] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar Usuário pelo identificador do Telegram(telegramId)
        /// </summary>
        /// <param name="telegramId"></param>
        /// <returns></returns>
        [Route("obter-usuario-por-telegramid/{telegramId}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuarioPorTelegramId(string telegramId)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterUsuarioPorTelegramId] => Request.: {new { TelegramId = telegramId }}");

                var usuario = await _application.ObterUsuarioPorTelegramId(telegramId);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterUsuarioPorTelegramId] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterUsuarioPorTelegramId] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Inserir/Criar Usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [Route("inserir-usuario")]
        [HttpPost]
        public async Task<IActionResult> InserirUsuario(InsereUsuarioViewModel usuario)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [InserirUsuario] => Request.: {JsonSerializer.Serialize(usuario)}");

                await _application.InserirUsuario(usuario);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [InserirUsuario] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", usuario);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [InserirUsuario] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Atualizar/Modificar Usuário existente
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [Route("atualizar-usuario")]
        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(AtualizaUsuarioViewModel usuario)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [AtualizarUsuario] => Request.: {JsonSerializer.Serialize(usuario)}");

                await _application.AtualizarUsuario(usuario);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [AtualizarONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [AtualizarONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Excluir/Deletar Usuário existente por identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("excluir-usuario/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ExcluirUsuario] => Request.: {new { Id = id }}");

                await _application.ExcluirUsuario(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ExcluirUsuario] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Usuário excluído com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ExcluirUsuario] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion [Usuario]

        #region [Evento]
        /// <summary>
        /// Obter/Consultar uma lista de Eventos por Geo Localização(Latitude+Longitude) e atualiza o Endereço do Usuario caso obtenha retorno
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="paginacao"></param>
        /// <returns></returns>
        [Route("{usuarioId}/obter-eventos-por-geo")]
        [HttpPut]
        public async Task<IActionResult> ObterEventosPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterEventosPorGeo] => Request.: {new { latitude, longitude }}");
                var eventos = await _application.ObterEventosPorGeo(usuarioId, latitude, longitude, paginacao);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterEventosPorGeo] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterEventosPorGeo] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Seguir Evento por Usuário
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{usuarioId}/seguir-evento/{id}")]
        [HttpPut]
        public async Task<IActionResult> SeguirEvento(int usuarioId, int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [SeguirEvento] => Request.: {new { UsuarioId = usuarioId, Id = id }}");

                await _application.SeguirEvento(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [SeguirEvento] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Evento seguido com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [SeguirEvento] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Deixar de seguir Evento por Usuário
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{usuarioId}/desseguir-evento/{id}")]
        [HttpPut]
        public async Task<IActionResult> DesseguirEvento(int usuarioId, int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [DesseguirEvento] => Request.: {new { UsuarioId = usuarioId, Id = id }}");

                await _application.DesseguirEvento(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [DesseguirEvento] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Evento desseguido com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [DesseguirEvento] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion [Evento]

        #region [ONG]
        /// <summary>
        /// Obter/Consultar uma lista de ONGs por Geo Localização(Latitude+Longitude) e atualiza o Endereço do Usuario caso obtenha retorno
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="paginacao"></param>
        /// <returns></returns>
        [Route("{usuarioId}/obter-ongs-por-geo")]
        [HttpPut]
        public async Task<IActionResult> ObterONGsPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterONGsPorGeo] => Request.: {new { latitude, longitude }}");
                var ongs = await _application.ObterONGsPorGeo(usuarioId, latitude, longitude, paginacao);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterONGsPorGeo] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(ongs);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterONGsPorGeo] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Seguir ONG por Usuário
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{usuarioId}/seguir-ong/{id}")]
        [HttpPut]
        public async Task<IActionResult> SeguirONG(int usuarioId, int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [SeguirONG] => Request.: {new { UsuarioId = usuarioId, Id = id }}");

                await _application.SeguirONG(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [SeguirONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("ONG seguida com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [SeguirONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Deixar de seguir ONG por Usuário
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{usuarioId}/desseguir-ong/{id}")]
        [HttpPut]
        public async Task<IActionResult> DesseguirONG(int usuarioId, int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [DesseguirONG] => Request.: {new { UsuarioId = usuarioId, Id = id }}");

                await _application.DesseguirONG(usuarioId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [DesseguirONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("ONG desseguida com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [DesseguirONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion [ONG]
    }
}