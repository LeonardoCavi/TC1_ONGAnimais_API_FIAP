using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.Validations.Usuario;
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
            var usuarioMap = _mapper.Map<Usuario>(usuario);
            await _service.AtualizarUsuario(usuarioMap);
        }

        public async Task ExcluirUsuario(int id)
        {
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
            if(id > 0)
            {
                var usuario = await _service.ObterUsuario(id);

                if (_notificador.TemNotificacao())
                    return null;

                return _mapper.Map<ObtemUsuarioViewModel>(usuario);
            }

            Notificar("Id: o id é inválido", TipoNotificacao.BadRequest);
            return null;
        }

        public async Task<ObtemUsuarioEventosViewModel> ObterUsuarioEventos(int id)
        {
            var usuarioevento = await _service.ObterUsuarioEventos(id);
            return _mapper.Map<ObtemUsuarioEventosViewModel>(usuarioevento);
        }

        public async Task<ObtemUsuarioONGsViewModel> ObterUsuarioONGs(int id)
        {
            var usuarioong = await _service.ObterUsuarioONGs(id);
            return _mapper.Map<ObtemUsuarioONGsViewModel>(usuarioong);
        }

        #endregion

        #region [Evento]

        public async Task SeguirEvento(int eventoId, int id)
        {
            await _service.SeguirEvento(eventoId, id);
        }

        public async Task DesseguirEvento(int eventoId, int id)
        {
            await _service.DesseguirEvento(eventoId, id);
        }

        #endregion

        #region [ONG]

        public async Task SeguirONG(int ongId, int id)
        {
            await _service.SeguirONG(ongId, id);
        }

        public async Task DesseguirONG(int ongId, int id)
        {
            await _service.DesseguirONG(ongId, id);
        }

        #endregion
    }
}