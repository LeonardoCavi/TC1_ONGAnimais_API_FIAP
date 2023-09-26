using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;

namespace ONGAnimaisAPI.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _uRepository;
        private readonly IONGRepository _oRepository;
        private readonly IEventoRepository _eRepository;

        public UsuarioService(IONGRepository oRepository,
                              IEventoRepository eRepository,
                              IUsuarioRepository uRepository)
        {
            this._oRepository = oRepository;
            this._eRepository = eRepository;
            this._uRepository = uRepository;
        }

        #region [Usuario]

        public async Task AtualizarUsuario(Usuario usuario)
        {
            var usuarioDB = await _uRepository.Obter(usuario.Id);
            if (usuarioDB != null)
            {
                usuarioDB.Nome = usuario.Nome;
                usuarioDB.Telefone = usuario.Telefone;
                usuarioDB.Endereco = usuario.Endereco;

                await _uRepository.Atualizar(usuarioDB);
            }
        }

        public async Task ExcluirUsuario(int id)
        {
            var usuario = await _uRepository.Obter(id);
            if (usuario != null)
            {
                await _uRepository.Excluir(usuario);
            }
        }

        public async Task InserirUsuario(Usuario usuario)
        {
            await _uRepository.Inserir(usuario);
        }

        public async Task<ICollection<Usuario>> ObterTodosUsuarios()
        {
            return await _uRepository.ObterTodos();
        }

        public async Task<Usuario> ObterUsuario(int id)
        {
            return await _uRepository.Obter(id);
        }

        public async Task<Usuario> ObterUsuarioEventos(int id)
        {
            return await _uRepository.ObterUsuarioEventos(id);
        }

        public async Task<Usuario> ObterUsuarioONGs(int id)
        {
            return await _uRepository.ObterUsuarioONGs(id);
        }

        #endregion [Usuario]

        #region [Evento]
        #endregion

        #region [ONG]
        #endregion
    }
}