using Microsoft.AspNetCore.Mvc;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.ONG;

namespace ONGAnimaisAPI.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ONGController : ControllerBase
    {
        private readonly IONGApplicationService _application;

        public ONGController(IONGApplicationService application)
        {
            this._application = application;
        }

        [Route("obter-ong/{id}")]
        [HttpGet]
        public async Task<IActionResult> ObterONG(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Identificador da ONG inválido. Tente novamente!");
                }

                var ong = await _application.ObterONG(id);

                if (ong != null)
                {
                    return Ok(ong);
                }
                else
                {
                    return NotFound("ONG não encontrada!");
                }
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

                if (ongs.Any())
                {
                    return Ok(ongs);
                }
                else
                {
                    return NotFound("Não existe ONGs cadastradas!");
                }
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
                if (id <= 0)
                {
                    return BadRequest("Identificador da ONG inválido. Tente novamente!");
                }

                var ong = await _application.ObterONGEventos(id);

                if (ong != null)
                {
                    return Ok(ong);
                }
                else
                {
                    return NotFound("ONG não encontrada!");
                }
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
                if (ong == null)
                {
                    return BadRequest("Dados da ONG incorretos!");
                }

                await _application.InserirONG(ong);
                return Created("", ong);
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
                if (ong == null)
                {
                    return BadRequest("Dados da ONG incorretos!");
                }
                if (ong.Id <= 0)
                {
                    return BadRequest("Identificador da ONG inválido. Tente novamente!");
                }

                await _application.AtualizarONG(ong);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }

        [Route("excluir-ong")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirONG(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Identificador da ONG inválido. Tente novamente!");
                }

                await _application.ExcluirONG(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERRO => " + ex.Message);
            }
        }
    }
}