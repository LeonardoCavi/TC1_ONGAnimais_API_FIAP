using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Repository
{
    public interface IONGRepository : IEntidadeBaseRepository<ONG>
    {
        Task<ONG> ObterONGEventos(int id);
        Task<ICollection<ONG>> ObterONGsPorCidade(string cidade, string uf, int paginacao = 0);
        Task<ICollection<ONG>> ObterONGsPorCidadeGeo(string cidade, string uf, int paginacao = 0);
    }
}