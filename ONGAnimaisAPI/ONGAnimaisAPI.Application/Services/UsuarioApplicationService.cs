using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.Validations;
using ONGAnimaisAPI.Application.Validations.Usuario;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Domain.Abstracts;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using ONGAnimaisAPI.Domain.Notifications;

namespace ONGAnimaisAPI.Application.Services
{
    public class UsuarioApplicationService : NotificadorContext, IUsuarioApplicationService
    {
        private readonly IUsuarioService _service;
        private readonly IMapper _mapper;

        public UsuarioApplicationService(IUsuarioService service,
                                         IMapper mapper,
                                         INotificador notificador)
            : base(notificador)
        {
            this._service = service;
            this._mapper = mapper;
        }

        #region [Usuario]

        public async Task AtualizarUsuario(AtualizaUsuarioViewModel usuario)
        {
            ExecutarValidacao(new AtualizaUsuarioValidation(), usuario);

            if (!_notificador.TemNotificacao())
            {
                var usuarioMap = _mapper.Map<Usuario>(usuario);
                await _service.AtualizarUsuario(usuarioMap);
            }
        }

        public async Task ExcluirUsuario(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (!_notificador.TemNotificacao())
                await _service.ExcluirUsuario(id);
        }

        public async Task InserirUsuario(InsereUsuarioViewModel usuario)
        {
            ExecutarValidacao(new InsereUsuarioValidation(), usuario);

            if (!_notificador.TemNotificacao())
            {
                var usuarioMap = _mapper.Map<Usuario>(usuario);
                await _service.InserirUsuario(usuarioMap);
            }
        }

        public async Task<ICollection<ObtemUsuarioViewModel>> ObterTodosUsuarios()
        {
            var usuarios = await _service.ObterTodosUsuarios();
            return _mapper.Map<List<ObtemUsuarioViewModel>>(usuarios);
        }

        public async Task<ObtemUsuarioViewModel> ObterUsuario(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (!_notificador.TemNotificacao())
            {
                var usuario = await _service.ObterUsuario(id);

                if (!_notificador.TemNotificacao())
                    return _mapper.Map<ObtemUsuarioViewModel>(usuario);
            }

            return null;
        }

        public async Task<ObtemUsuarioEventosViewModel> ObterUsuarioEventos(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (!_notificador.TemNotificacao())
            {
                var usuarioevento = await _service.ObterUsuarioEventos(id);

                if (!_notificador.TemNotificacao())
                    return _mapper.Map<ObtemUsuarioEventosViewModel>(usuarioevento);
            }

            return null;
        }

        public async Task<ObtemUsuarioONGsViewModel> ObterUsuarioONGs(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (!_notificador.TemNotificacao())
            {
                var usuarioOng = await _service.ObterUsuarioONGs(id);

                if (!_notificador.TemNotificacao())
                    return _mapper.Map<ObtemUsuarioONGsViewModel>(usuarioOng);
            }

            return null;
        }

        public async Task<ObtemUsuarioTelegramViewModel> ObterUsuarioPorTelegramId(string telegramId)
        {
            ExecutarValidacao(new TelegramIdValidation(), telegramId);

            if (!_notificador.TemNotificacao())
            {
                var usuario = await _service.ObterUsuarioPorTelegramId(telegramId);

                if (!_notificador.TemNotificacao())
                    return _mapper.Map<ObtemUsuarioTelegramViewModel>(usuario);
            }

            return null;
        }

        #endregion

        #region [Evento]
        public async Task<ICollection<ObtemEventoGeoViewModel>> ObterEventosPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0)
        {
            ExecutarValidacao(new IdValidation(), usuarioId);
            ExecutarValidacao(new LatitudeLongitudeValidation(), (latitude, longitude));

            if (!_notificador.TemNotificacao())
            {
                var eventos = await _service.ObterEventosPorGeo(usuarioId, latitude, longitude, paginacao);

                return _mapper.Map<ICollection<ObtemEventoGeoViewModel>>(eventos);
            }

            return null;
        }


        public async Task SeguirEvento(int usuarioId, int id)
        {
            ExecutarValidacao(new IdUsuarioValidation(), (usuarioId, id));

            if (!_notificador.TemNotificacao())
                await _service.SeguirEvento(usuarioId, id);
        }

        public async Task DesseguirEvento(int usuarioId, int id)
        {
            ExecutarValidacao(new IdUsuarioValidation(), (usuarioId, id));

            if (!_notificador.TemNotificacao())
                await _service.DesseguirEvento(usuarioId, id);
        }

        #endregion

        #region [ONG]
        public async Task<ICollection<ObtemONGGeoViewModel>> ObterONGsPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0)
        {
            ExecutarValidacao(new IdValidation(), usuarioId);
            ExecutarValidacao(new LatitudeLongitudeValidation(), (latitude, longitude));

            if (!_notificador.TemNotificacao())
            {
                var ongs = await _service.ObterONGsPorGeo(usuarioId, latitude, longitude, paginacao);

                return _mapper.Map<ICollection<ObtemONGGeoViewModel>>(ongs);
            }

            return null;
        }
        public async Task SeguirONG(int usuarioId, int id)
        {
            ExecutarValidacao(new IdUsuarioValidation(), (usuarioId, id));

            if (!_notificador.TemNotificacao())
                await _service.SeguirONG(usuarioId, id);
        }

        public async Task DesseguirONG(int usuarioId, int id)
        {
            ExecutarValidacao(new IdUsuarioValidation(), (usuarioId, id));

            if (!_notificador.TemNotificacao())
                await _service.DesseguirONG(usuarioId, id);
        }

        #endregion
    }
}