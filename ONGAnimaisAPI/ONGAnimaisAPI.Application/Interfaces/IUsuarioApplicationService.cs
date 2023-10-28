using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;

namespace ONGAnimaisAPI.Application.Interfaces
{
    public interface IUsuarioApplicationService
    {
        #region [Usuario]
        Task InserirUsuario(InsereUsuarioViewModel usuario);

        Task<ObtemUsuarioViewModel> ObterUsuario(int id);

        Task<ICollection<ObtemUsuarioViewModel>> ObterTodosUsuarios();

        Task<ObtemUsuarioEventosViewModel> ObterUsuarioEventos(int id);

        Task<ObtemUsuarioONGsViewModel> ObterUsuarioONGs(int id);

        Task<ObtemUsuarioTelegramViewModel> ObterUsuarioPorTelegramId(string telegramId);

        Task AtualizarUsuario(AtualizaUsuarioViewModel usuario);

        Task ExcluirUsuario(int id);

        #endregion

        #region [Evento]
        Task<ICollection<ObtemEventoGeoViewModel>> ObterEventosPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0);
        Task SeguirEvento(int usuarioId, int id);

        Task DesseguirEvento(int usuarioId, int id);

        #endregion

        #region [ONG]
        Task<ICollection<ObtemONGGeoViewModel>> ObterONGsPorGeo(int usuarioId, decimal latitude, decimal longitude, int paginacao = 0);
        Task SeguirONG(int usuarioId, int id);

        Task DesseguirONG(int usuarioId, int id);

        #endregion
    }
}