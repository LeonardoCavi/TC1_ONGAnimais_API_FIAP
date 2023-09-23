using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Repository
{
    public interface IONGRepository : IEntidadeBaseRepository<ONG>
    {
        Task<ONG> ObterONGEventos(int id);
    }
}