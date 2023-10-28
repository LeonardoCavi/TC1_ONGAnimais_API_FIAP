using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Domain.Interfaces.Repository
{
    public interface IUsuarioRepository : IEntidadeBaseRepository<Usuario>
    {
        Task<Usuario> ObterUsuarioEventos(int id);
        Task<Usuario> ObterUsuarioONGs(int id);
        Task<Usuario> ObterUsuarioPorTelegramId(string telegramId);
    }
}