using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task InserirUsuario(Usuario usuario);

        Task<Usuario> ObterUsuario(int id);

        Task<ICollection<Usuario>> ObterTodosUsuarios();

        Task<Usuario> ObterUsuarioEventos(int id);

        Task<Usuario> ObterUsuarioONGs(int id);

        Task AtualizarUsuario(Usuario usuario);

        Task ExcluirUsuario(int id);
    }
}