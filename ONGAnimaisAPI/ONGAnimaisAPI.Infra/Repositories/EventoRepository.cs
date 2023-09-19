using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class EventoRepository : EntidadeBaseRepository<Evento>, IEventoRepository
    {
        public EventoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}