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

        Task<Usuario> ObterUsuarioPorTelegramId(string telegramId);

        Task AtualizarUsuario(Usuario usuario);

        Task ExcluirUsuario(int id);

        #endregion

        #region [ONG]
        Task<ICollection<ONG>> ObterONGsPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0);
        Task SeguirONG(int usuarioId, int id);
        Task DesseguirONG(int usuarioId, int id);

        #endregion

        #region [Evento]
        Task<ICollection<Evento>> ObterEventosPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0);
        Task SeguirEvento(int usuarioId, int id);
        Task DesseguirEvento(int usuarioId, int id);

        #endregion
    }
}