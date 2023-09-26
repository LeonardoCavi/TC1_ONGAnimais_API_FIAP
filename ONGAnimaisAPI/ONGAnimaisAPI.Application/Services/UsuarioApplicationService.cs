using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Services;

namespace ONGAnimaisAPI.Application.Services
{
    public class UsuarioApplicationService : IUsuarioApplicationService
    {
        private readonly IUsuarioService _service;
        private readonly IMapper _mapper;

        public UsuarioApplicationService(IUsuarioService service,
                                         IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }

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
            var usuarioMap = _mapper.Map<Usuario>(usuario);
            await _service.InserirUsuario(usuarioMap);
        }

        public async Task<ICollection<ObtemUsuarioViewModel>> ObterTodosUsuarios()
        {
            var usuarios = await _service.ObterTodosUsuarios();
            return _mapper.Map<List<ObtemUsuarioViewModel>>(usuarios);
        }

        public async Task<ObtemUsuarioViewModel> ObterUsuario(int id)
        {
            var usuario = await _service.ObterUsuario(id);
            return _mapper.Map<ObtemUsuarioViewModel>(usuario);
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
    }
}