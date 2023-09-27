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

        public async Task SeguirEvento(int eventoId, int id)
        {
            var usuario = await _uRepository.ObterUsuarioEventos(id);
            if (usuario != null)
            {
                if(!usuario.EventosSeguidos.Any(e => e.Id == eventoId))
                {
                    var evento = await _eRepository.Obter(eventoId);
                    if (evento != null)
                    {
                        usuario.EventosSeguidos.Add(evento);
                        await _uRepository.Atualizar(usuario);
                    }                         
                }
            }
        }

        public async Task DesseguirEvento(int eventoId, int id)
        {
            var usuario = await _uRepository.ObterUsuarioEventos(id);
            if (usuario != null)
            {
                var evento = usuario.EventosSeguidos.FirstOrDefault(e => e.Id == eventoId);
                if (evento != null)
                {
                    usuario.EventosSeguidos.Remove(evento);
                    await _uRepository.Atualizar(usuario);
                }
            }
        }

        #endregion

        #region [ONG]

        public async Task SeguirONG(int ongId, int id)
        {
            var usuario = await _uRepository.ObterUsuarioONGs(id);
            if (usuario != null)
            {
                if (!usuario.ONGsSeguidas.Any(o => o.Id == ongId))
                {
                    var ong = await _oRepository.Obter(ongId);
                    if (ong != null)
                    {
                        usuario.ONGsSeguidas.Add(ong);
                        await _uRepository.Atualizar(usuario);
                    }
                }
            }
        }

        public async Task DesseguirONG(int ongId, int id)
        {
            var usuario = await _uRepository.ObterUsuarioONGs(id);
            if (usuario != null)
            {
                var ong = usuario.ONGsSeguidas.FirstOrDefault(o => o.Id == ongId);
                if (ong != null)
                {
                    usuario.ONGsSeguidas.Remove(ong);
                    await _uRepository.Atualizar(usuario);
                }
            }
        }

        #endregion
    }
}