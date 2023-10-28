using ONGAnimaisAPI.Domain.Abstracts;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using ONGAnimaisAPI.Domain.Interfaces.Utility;
using ONGAnimaisAPI.Domain.Notifications;

namespace ONGAnimaisAPI.Domain.Services
{
    public class UsuarioService : NotificadorContext, IUsuarioService
    {
        private readonly IUsuarioRepository _uRepository;
        private readonly IONGRepository _oRepository;
        private readonly IEventoRepository _eRepository;
        private readonly IGeocodingAPIHttpClient _geoHttp;

        public UsuarioService(IONGRepository oRepository,
                              IEventoRepository eRepository,
                              IUsuarioRepository uRepository,
                              IGeocodingAPIHttpClient geoHttp,
                              INotificador notificador): base(notificador)
        {
            this._oRepository = oRepository;
            this._eRepository = eRepository;
            this._uRepository = uRepository;
            this._geoHttp = geoHttp;
        }

        #region [Usuario]

        public async Task AtualizarUsuario(Usuario usuario)
        {
            var usuarioDB = await ObterUsuario(usuario.Id);

            if (!_notificador.TemNotificacao())
            {
                usuarioDB.Nome = usuario.Nome;
                usuarioDB.Telefone = usuario.Telefone;
                usuarioDB.Endereco = usuario.Endereco;

                await _uRepository.Atualizar(usuarioDB);
            }
        }

        public async Task ExcluirUsuario(int id)
        {
            var usuario = await ObterUsuario(id);

            if (!_notificador.TemNotificacao())
                await _uRepository.Excluir(usuario);
        }

        public async Task InserirUsuario(Usuario usuario)
        {
            await _uRepository.Inserir(usuario);
        }

        public async Task<ICollection<Usuario>> ObterTodosUsuarios()
        {
            var usuarios = await _uRepository.ObterTodos();

            if (!usuarios.Any())
                Notificar($"Usuario: não existem usuários cadastrados", TipoNotificacao.NotFound);

            return usuarios;
        }

        public async Task<Usuario> ObterUsuario(int id)
        {
            var usuario = await _uRepository.Obter(id);

            if (usuario is null)
                Notificar($"Usuario: usuário com id '{id}' não existe", TipoNotificacao.NotFound);

            return usuario;
        }

        public async Task<Usuario> ObterUsuarioEventos(int id)
        {
            var usuarioEventos = await _uRepository.ObterUsuarioEventos(id);

            if (usuarioEventos is null)
                Notificar($"Usuario: usuário com id '{id}' não existe", TipoNotificacao.NotFound);

            return usuarioEventos;
        }

        public async Task<Usuario> ObterUsuarioONGs(int id)
        {
            var usuarioOngs = await _uRepository.ObterUsuarioONGs(id);

            if (usuarioOngs is null)
                Notificar($"Usuario: usuário com id '{id}' não existe", TipoNotificacao.NotFound);

            return usuarioOngs;
        }

        public async Task<Usuario> ObterUsuarioPorTelegramId(string telegramId)
        {
            var usuario = await _uRepository.ObterUsuarioPorTelegramId(telegramId);

            if (usuario is null)
                Notificar($"Usuario: usuário com Telegram Id '{telegramId}' não existe", TipoNotificacao.NotFound);

            return usuario;

        }

        private async Task AtualizarUsuarioEndereco(int id, Endereco endereco, GeoLocalizacao geoLocalizacao)
        {
            if (id > 0)
            {
                var usuario = await _uRepository.Obter(id);

                if (usuario != null)
                {
                    usuario.Endereco = endereco;
                    usuario.GeoLocalizacao = geoLocalizacao;

                    await _uRepository.Atualizar(usuario);
                }
            }
        }

        #endregion [Usuario]

        #region [Evento]

        public async Task<ICollection<Evento>> ObterEventosPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0)
        {
            var endereco = await _geoHttp.BuscarEnderecoPorGeoLocalizacao(latitude, longitude);

            if (endereco == null)
            {
                Notificar($"Evento: o endereço informado não foi encontrado ou é invalido", TipoNotificacao.NotFound);
                return null;
            }

            var eventos = await _eRepository.ObterEventosPorGeo(endereco.Cidade, endereco.UF, paginacao);

            if (eventos.Any())
                await AtualizarUsuarioEndereco(usuarioId, endereco, new() { Latitude = latitude, Longitude = longitude });
            else
                Notificar($"Evento: não existem eventos cadastrados em sua cidade", TipoNotificacao.NotFound);

            return eventos;
        }

        public async Task SeguirEvento(int usuarioId, int id)
        {
            var usuarioEventos = await ObterUsuarioEventos(usuarioId);

            if (!_notificador.TemNotificacao())
            {
                if (usuarioEventos.EventosSeguidos.Any(e => e.Id == id))
                    Notificar($"Usuario: usuário com id '{usuarioId}' já está seguindo o evento com id '{id}'", TipoNotificacao.Conflict);
                else
                {
                    var evento = await _eRepository.Obter(id);

                    if (evento is null)
                        Notificar($"Evento: evento com id '{id}' não existe", TipoNotificacao.NotFound);
                    else
                    {
                        usuarioEventos.EventosSeguidos.Add(evento);
                        await _uRepository.Atualizar(usuarioEventos);
                    }
                }
            }
        }

        public async Task DesseguirEvento(int usuarioId, int id)
        {
            var usuarioEventos = await ObterUsuarioEventos(usuarioId);

            if (!_notificador.TemNotificacao())
            {
                var evento = usuarioEventos.EventosSeguidos.FirstOrDefault(e => e.Id == id);

                if (evento is null)
                    Notificar($"Usuario: usuário com id '{usuarioId}' já não está seguindo o evento com id '{id}'", TipoNotificacao.Conflict);
                else 
                {
                    usuarioEventos.EventosSeguidos.Remove(evento);
                    await _uRepository.Atualizar(usuarioEventos);
                }
            }
        }

        #endregion

        #region [ONG]
        public async Task<ICollection<ONG>> ObterONGsPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0)
        {
            var endereco = await _geoHttp.BuscarEnderecoPorGeoLocalizacao(latitude, longitude);

            if (endereco == null)
            {
                Notificar($"ONG: o endereço informado não foi encontrado ou é invalido", TipoNotificacao.NotFound);
                return null;
            }

            var ongs = await _oRepository.ObterONGsPorGeo(endereco.Cidade, endereco.UF, paginacao);

            if (ongs.Any())
                await AtualizarUsuarioEndereco(usuarioId, endereco, new() { Latitude = latitude, Longitude = longitude });
            else
                Notificar($"ONG: não existem ongs cadastradas em sua cidade", TipoNotificacao.NotFound);

            return ongs;
        }

        public async Task SeguirONG(int usuarioId, int id)
        {
            var usuarioOngs = await ObterUsuarioONGs(usuarioId);

            if (!_notificador.TemNotificacao())
            {
                if (usuarioOngs.ONGsSeguidas.Any(e => e.Id == id))
                    Notificar($"Usuario: usuário com id '{usuarioId}' já está seguindo a ong com id '{id}'", TipoNotificacao.Conflict);
                else
                {
                    var ong = await _oRepository.Obter(id);

                    if (ong is null)
                        Notificar($"ONG: ong com id '{id}' não existe", TipoNotificacao.NotFound);
                    else
                    {
                        usuarioOngs.ONGsSeguidas.Add(ong);
                        await _uRepository.Atualizar(usuarioOngs);
                    }
                }
            }
        }

        public async Task DesseguirONG(int usuarioId, int id)
        {
            var usuarioOngs = await ObterUsuarioONGs(usuarioId);

            if (!_notificador.TemNotificacao())
            {
                var ong = usuarioOngs.ONGsSeguidas.FirstOrDefault(o => o.Id == id);

                if (ong is null)
                    Notificar($"Usuario: usuário com id '{usuarioId}' já não está seguindo a ong com id '{id}'", TipoNotificacao.Conflict);
                else
                {
                    usuarioOngs.ONGsSeguidas.Remove(ong);
                    await _uRepository.Atualizar(usuarioOngs);
                }
            }
        }

        #endregion
    }
}