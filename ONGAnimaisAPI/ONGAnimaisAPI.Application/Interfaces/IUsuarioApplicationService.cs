using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;

namespace ONGAnimaisAPI.Application.Interfaces
{
    public interface IUsuarioApplicationService
    {
        Task InserirUsuario(InsereUsuarioViewModel usuario);

        Task<ObtemUsuarioViewModel> ObterUsuario(int id);

        Task<ICollection<ObtemUsuarioViewModel>> ObterTodosUsuarios();

        Task<ObtemUsuarioEventosViewModel> ObterUsuarioEventos(int id);

        Task<ObtemUsuarioONGsViewModel> ObterUsuarioONGs(int id);

        Task AtualizarUsuario(AtualizaUsuarioViewModel usuario);

        Task ExcluirUsuario(int id);
    }
}