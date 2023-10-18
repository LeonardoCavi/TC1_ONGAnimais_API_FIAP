using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repository;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class EventoRepository : EntidadeBaseRepository<Evento>, IEventoRepository
    {
        public EventoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Evento>> ObterEventosPorCidade(string cidade, string uf, int paginacao = 0)
        {
            return await _dBSet
                .Where(e => e.Endereco.Cidade == cidade &&
            e.Endereco.UF == uf && e.Data >= DateTime.Now)
                .OrderBy(e => e.Data)
                .Skip(paginacao * 5)
                .Take(5)
                .ToListAsync();
        }

    }
}