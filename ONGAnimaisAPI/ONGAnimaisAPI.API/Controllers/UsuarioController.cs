using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplicationService _application;

        public UsuarioController(IUsuarioApplicationService application)
        {
            this._application = application;
        }

        [Route("obter-usuario/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }

                var usuario = await _application.ObterUsuario(id);

                if (usuario != null)
                {
                    return Ok(usuario);
                }
                else
                {
                    return NotFound("Usuário não encontrada!");
                }
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

                if (usuarios.Any())
                {
                    return Ok(usuarios);
                }
                else
                {
                    return NotFound("Não existe Usuários cadastrados!");
                }
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
                if (id <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }

                var usuario = await _application.ObterUsuarioEventos(id);

                if (usuario != null)
                {
                    return Ok(usuario);
                }
                else
                {
                    return NotFound("Usuário não encontrado!");
                }
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
                if (id <= 0)
                {
                    return BadRequest("Identificador do Usuário inválido. Tente novamente!");
                }

                var usuario = await _application.ObterUsuarioONGs(id);

                if (usuario != null)
                {
                    return Ok(usuario);
                }
                else
                {
                    return NotFound("Usuário não encontrado!");
                }
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
                if (usuario == null)
                {
                    return BadRequest("Dados do Usuário incorretos!");
                }

                await _application.InserirUsuario(usuario);
                return Created("", usuario);
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
    }
}
