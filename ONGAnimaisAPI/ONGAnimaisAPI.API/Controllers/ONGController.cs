using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using System.Text.Json;

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
        private readonly ILogger<ONGController> _logger;
        private readonly string _className = typeof(ONGController).Name;

        public ONGController(IONGApplicationService application,
                             IMapper mapper,
                             INotificador notificador,
                             ILogger<ONGController> logger)
        {
            this._application = application;
            this._mapper = mapper;
            this._notificador = notificador;
            this._logger = logger;
        }

        #region [ONG]

        /// <summary>
        /// Obter/Consultar ONG pelo identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("obter-ong/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterONG(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterONG] => Request.: {new { Id = id }}");

                var ong = await _application.ObterONG(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(ong);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar todas ONGs
        /// </summary>
        /// <returns></returns>
        [Route("obter-todas-ong")]
        [HttpGet]
        private async Task<IActionResult> ObterTodasONG()
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterTodasONG] => Request.: n/a");
                var ongs = await _application.ObterTodasONG();

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterTodasONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(ongs);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterTodasONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar ONGs por Cidade e UF
        /// </summary>
        /// <param name="ongcidade"></param>
        /// <returns></returns>
        [Route("obter-ongs-por-cidade")]
        [HttpGet]
        public async Task<IActionResult> ObterONGsPorCidade([FromQuery] BuscaONGCidadeViewModel ongcidade, int paginacao = 0)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterONGsPorCidade] => Request.: {JsonSerializer.Serialize(ongcidade)}");
                var ongs = await _application.ObterONGsPorCidade(ongcidade, paginacao);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterONGsPorCidade] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(ongs);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterONGsPorCidade] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar ONG e seus Eventos pelo identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("obter-ong-eventos/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterONGEventos(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterONGEventos] => Request.: {new { Id = id }}");

                var ong = await _application.ObterONGEventos(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterONGEventos] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(ong);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterONGEventos] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Inserir/Criar ONG
        /// </summary>
        /// <param name="ong"></param>
        /// <returns></returns>
        [Route("inserir-ong")]
        [HttpPost]
        public async Task<IActionResult> InserirONG(InsereONGViewModel ong)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [InserirONG] => Request.: {JsonSerializer.Serialize(ong)}");

                await _application.InserirONG(ong);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [InserirONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", ong);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [InserirONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Atualizar/Modificar ONG existente
        /// </summary>
        /// <param name="ong"></param>
        /// <returns></returns>
        [Route("atualizar-ong")]
        [HttpPut]
        public async Task<IActionResult> AtualizarONG(AtualizaONGViewModel ong)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [AtualizarONG] => Request.: {JsonSerializer.Serialize(ong)}");

                await _application.AtualizarONG(ong);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [AtualizarONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("ONG atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [AtualizarONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Excluir/Deletar ONG existente por identificador(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("excluir-ong/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirONG(int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ExcluirONG] => Request.: {new { Id = id }}");

                await _application.ExcluirONG(id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ExcluirONG] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("ONG excluída com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ExcluirONG] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion [ONG]

        #region [Evento]

        /// <summary>
        /// Obter/Consultar Evento pelo identificador(id)
        /// </summary>
        /// <param name="ongId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{ongId}/obter-evento/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterEvento(int ongId, int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterEvento] => Request.: {new { ONGId = ongId, Id = id }}");

                var evento = await _application.ObterEvento(ongId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterEvento] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterEvento] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Obter/Consultar Eventos por Cidade e UF
        /// </summary>
        /// <param name="eventocidade"></param>
        /// <param name="paginacao"></param>
        /// <returns></returns>
        [Route("obter-eventos-por-cidade")]
        [HttpGet]
        public async Task<IActionResult> ObterEventosPorCidade([FromQuery] BuscaEventoCidadeViewModel eventocidade, int paginacao = 0)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ObterEventosPorCidade] => Request.: {JsonSerializer.Serialize(eventocidade)}");
                var ongs = await _application.ObterEventosPorCidade(eventocidade, paginacao);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ObterEventosPorCidade] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok(ongs);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ObterEventosPorCidade] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Inserir/Criar Evento associado à uma ONG existente
        /// </summary>
        /// <param name="ongId"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        [Route("{ongId}/inserir-evento")]
        [HttpPost]
        public async Task<IActionResult> InserirEvento(int ongId, InsereEventoViewModel evento)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [InserirEvento] => Request.: {new { ONGId = ongId }},{JsonSerializer.Serialize(evento)}");

                await _application.InserirEvento(ongId, evento);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [InserirEvento] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Created("", evento);
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [InserirEvento] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Atualizar/Modificar Evento existente
        /// </summary>
        /// <param name="ongId"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        [Route("{ongId}/atualizar-evento")]
        [HttpPut]
        public async Task<IActionResult> AtualizarEvento(int ongId, AtualizaEventoViewModel evento)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [AtualizarEvento] => Request.: {new { ONGId = ongId }},{JsonSerializer.Serialize(evento)}");

                await _application.AtualizarEvento(ongId, evento);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [AtualizarEvento] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Evento atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [AtualizarEvento] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        /// <summary>
        /// Excluir/Deletar Evento existente por identificador(id)
        /// </summary>
        /// <param name="ongId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{ongId}/excluir-evento/{id}")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirEvento(int ongId, int id)
        {
            try
            {
                _logger.LogInformation($"[{_className}] - [ExcluirEvento] => Request.: {new { ONGId = ongId, Id = id }}");

                await _application.ExcluirEvento(ongId, id);

                if (_notificador.TemNotificacao())
                {
                    var resposta = _mapper.Map<ErroViewModel>(_notificador.ObterNotificacoes());
                    _logger.LogWarning($"[{_className}] - [ExcluirEvento] => Notificações.: {JsonSerializer.Serialize(resposta)}");
                    return StatusCode(resposta.StatusCode, resposta);
                }

                return Ok("Evento excluído com sucesso!");
            }
            catch (Exception ex)
            {
                var resposta = _mapper.Map<ErroViewModel>(ex);
                _logger.LogError($"[{_className}] - [ExcluirEvento] => Exception.: {ex.Message}");
                return StatusCode(resposta.StatusCode, resposta);
            }
        }

        #endregion [Evento]
    }
}