using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Repository
{
    public interface IUsuarioRepository : IEntidadeBaseRepository<Usuario>
    {
        Task<Usuario> ObterUsuarioEventos(int id);
        Task<Usuario> ObterUsuarioONGs(int id);
        Task<Usuario> ObterUsuarioPorTelegramId(string telegramId);
    }
}