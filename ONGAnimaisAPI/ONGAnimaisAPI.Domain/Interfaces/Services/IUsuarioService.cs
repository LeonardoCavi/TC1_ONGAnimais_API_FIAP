using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        #region [Usuario]

        Task InserirUsuario(Usuario usuario);

        Task<Usuario> ObterUsuario(int id);

        Task<ICollection<Usuario>> ObterTodosUsuarios();

        Task<Usuario> ObterUsuarioEventos(int id);

        Task<Usuario> ObterUsuarioONGs(int id);

        Task AtualizarUsuario(Usuario usuario);

        Task ExcluirUsuario(int id);

        #endregion

        #region [ONG]

        Task SeguirONG(int ongId, int id);
        Task DesseguirONG(int ongId, int id);

        #endregion

        #region [Evento]

        Task SeguirEvento(int eventoId, int id);
        Task DesseguirEvento(int eventoId, int id);

        #endregion
    }
}