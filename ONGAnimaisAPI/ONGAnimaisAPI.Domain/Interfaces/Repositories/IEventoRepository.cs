using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Repository
{
    public interface IEventoRepository : IEntidadeBaseRepository<Evento>
    {
        Task<ICollection<Evento>> ObterEventosPorCidade(string cidade, string uf, int paginacao = 0);
    }
}