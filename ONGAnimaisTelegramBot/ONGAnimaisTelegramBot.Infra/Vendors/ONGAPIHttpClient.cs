using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Infra.Utility;
using ONGAnimaisTelegramBot.Infra.Vendors.Config;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System.Text.Json;

namespace ONGAnimaisTelegramBot.Infra.Vendors
{
    public class ONGAPIHttpClient : IONGAPIHttpClient
    {
        private static string Token;
        private static DateTime ExpiraToken;
        private static bool gerandoToken = false;
        private readonly ILogger<ONGAPIHttpClient> _logger;
        private readonly string ClassName = typeof(ONGAPIHttpClient).Name;
        private readonly HttpHelp _httpHelp;
        private readonly ONGApiConfig _apiConfig;

        public ONGAPIHttpClient(ILogger<ONGAPIHttpClient> logger,
                                HttpHelp httpHelp,
                                ONGApiConfig apiConfig)
        {
            _logger = logger;
            _httpHelp = httpHelp;
            _apiConfig = apiConfig;
        }

        public async Task GerarToken()
        {
            try
            {
                _logger.LogInformation($"{ClassName}:GerarToken => Geração de Token de acesso");
                string url = _apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.TokenEndpoint;

                var json = JsonSerializer
                    .Serialize(new
                    {
                        usuario = _apiConfig.Usuario,
                        senha = _apiConfig.Senha
                    });

                var result = await _httpHelp.Send(url, json, VerboHttp.Post, null);

                if (result.Code == CodeHttp.Sucess)
                {
                    var autenticacao = JsonSerializer.Deserialize<Autenticacao>(result.Received);
                    Token = autenticacao.Token;
                    //ExpiraToken = autenticacao.expiration;
                    ExpiraToken = autenticacao.Expericao.ToLocalTime();
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:GerarToken => Erro na geração de Token de acesso. Response => {result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:GerarToken => Exception => {ex.Message}");
            }
        }

        public async Task<ONG> ObterOng(int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterOng => Request => {new { id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterONGEndpoint, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var ong = JsonSerializer.Deserialize<ONG>(result.Received);
                    return ong;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterOng => Erro na obtenção de ONG. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterOng => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<ONG>> ObterOngsCidade(string cidade, string uf, int paginacao = 0)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterOngsCidade => Request => {new { cidade, uf, paginacao }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterONGsPorCidadeEndpoint, cidade, uf, paginacao);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var ongs = JsonSerializer.Deserialize<ICollection<ONG>>(result.Received);
                    return ongs;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterOngsCidade => Erro na obtenção de ONGs por Cidade e UF. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterOngsCidade => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<ONG>> ObterOngsCidadeGeo(string cidade, string uf, int paginacao = 0)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterOngsCidadeGeo => Request => {new { cidade, uf, paginacao }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterONGsPorCidadeGeoEndpoint, cidade, uf, paginacao);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var ongs = JsonSerializer.Deserialize<ICollection<ONG>>(result.Received);
                    return ongs;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterOngsCidadeGeo => Erro na obtenção de ONGs por Cidade e UF. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterOngsCidadeGeo => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<ONG> ObterONGEventos(int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterONGEventos => Request => {new { id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterONGEventosEndpoint, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var ongEventos = JsonSerializer.Deserialize<ONG>(result.Received);
                    return ongEventos;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterONGEventos => Erro na obtenção de ONG e seus Eventos. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterONGEventos => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<Evento>> ObterEventosCidade(string cidade, string uf, int paginacao = 0)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterEventosCidade => Request => {new { cidade, uf, paginacao }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterEventosPorCidadeEndpoint, cidade, uf, paginacao);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var eventos = JsonSerializer.Deserialize<ICollection<Evento>>(result.Received);
                    return eventos;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterEventosCidade => Erro na obtenção de Eventos por Cidade e UF. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterEventosCidade => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<Evento>> ObterEventosCidadeGeo(string cidade, string uf, int paginacao = 0)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterEventosCidadeGeo => Request => {new { cidade, uf, paginacao }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterEventosPorCidadeGeoEndpoint, cidade, uf, paginacao);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var eventos = JsonSerializer.Deserialize<ICollection<Evento>>(result.Received);
                    return eventos;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterEventosCidadeGeo => Erro na obtenção de Eventos por Cidade e UF. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterEventosCidadeGeo => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<Evento> ObterEvento(int ongId, int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterEvento => Request => {new { ongId, id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterEventoEndpoint, ongId, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var evento = JsonSerializer.Deserialize<Evento>(result.Received);
                    return evento;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterEvento => Erro na obtenção de Evento. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterEvento => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> ObterUsuario(int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterUsuario => Request => {new { id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterUsuarioEndpoint, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var usuario = JsonSerializer.Deserialize<Usuario>(result.Received);
                    return usuario;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterUsuario => Erro na obtenção de Usuário. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterUsuario => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> ObterUsuarioPorTelegramId(string telegramId)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterUsuarioPorTelegramId => Request => {new { telegramId }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterUsuarioPorTelegramIdEndpoint, telegramId);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var usuario = JsonSerializer.Deserialize<Usuario>(result.Received);
                    return usuario;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterUsuarioPorTelegramId => Erro na obtenção de Usuário. Response => {JsonSerializer.Serialize(result)}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterUsuarioPorTelegramId => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> ObterUsuarioEventos(int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterUsuarioEventos => Request => {new { id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterUsuarioEventosEndpoint, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var usuarioEventos = JsonSerializer.Deserialize<Usuario>(result.Received);
                    return usuarioEventos;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterUsuarioEventos => Erro na obtenção de Usuário e seus Eventos seguidos. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterUsuarioEventos => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> ObterUsuarioONGs(int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:ObterUsuarioONGs => Request => {new { id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.ObterUsuarioONGsEndpoint, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Get, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var usuarioONGs = JsonSerializer.Deserialize<Usuario>(result.Received);
                    return usuarioONGs;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:ObterUsuarioONGs => Erro na obtenção de Usuário e suas ONGs seguidas. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:ObterUsuarioONGs => Exception => {ex.Message}");
                return null;
            }
        }

        public async Task<bool> SeguirEvento(int usuarioId, int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:SeguirEvento => Request => {new { usuarioId, id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.SeguirEventoEndpoint, usuarioId, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Put, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:SeguirEvento => Erro ao seguir Evento por Usuário. Response => {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:SeguirEvento => Exception => {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DesseguirEvento(int usuarioId, int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:DesseguirEvento => Request => {new { usuarioId, id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.DesseguirEventoEndpoint, usuarioId, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Put, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:DesseguirEvento => Erro ao desseguir Evento por Usuário. Response => {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:DesseguirEvento => Exception => {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SeguirONG(int usuarioId, int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:SeguirONG => Request => {new { usuarioId, id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.SeguirONGEndpoint, usuarioId, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Put, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:SeguirONG => Erro ao seguir ONG por Usuário. Response => {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:SeguirONG => Exception => {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DesseguirONG(int usuarioId, int id)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:DesseguirONG => Request => {new { usuarioId, id }}");
                string url = string.Format(_apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.DesseguirONGEndpoint, usuarioId, id);

                var result = await _httpHelp.Send(url, null, VerboHttp.Put, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:DesseguirONG => Erro ao desseguir ONG por Usuário. Response => {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:DesseguirONG => Exception => {ex.Message}");
                return false;
            }
        }

        public async Task<Usuario> InserirUsuario(Usuario usuario)
        {
            try
            {
                await VerificaVidaToken();

                _logger.LogInformation($"{ClassName}:InserirUsuario => Request => {JsonSerializer.Serialize(usuario)}");
                string url = _apiConfig.Endpoints.BaseUri + _apiConfig.Endpoints.InserirUsuarioEndpoint;

                var json = JsonSerializer.Serialize(usuario);

                var result = await _httpHelp.Send(url, json, VerboHttp.Post, AddHeaders());
                if (result.Code == CodeHttp.Sucess)
                {
                    var usuarioResult = JsonSerializer.Deserialize<Usuario>(result.Received);
                    return usuarioResult;
                }
                else
                {
                    _logger.LogWarning($"{ClassName}:InserirUsuario => Erro ao inserir Usuário. Response => {result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ClassName}:InserirUsuario => Exception => {ex.Message}");
                return null;
            }
        }

        private async Task VerificaVidaToken()
        {
            DateTime agora = DateTime.Now;
            agora = agora.AddMinutes(40);
            if (gerandoToken != true)
            {
                gerandoToken = true;
                if (ExpiraToken <= agora)
                {
                    await GerarToken();
                }
                gerandoToken = false;
            }
        }

        private Dictionary<string, string> AddHeaders()
        {
            return new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + Token}
            };
        }
    }
}