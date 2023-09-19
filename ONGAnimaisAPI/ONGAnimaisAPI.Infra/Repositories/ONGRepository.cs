using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class ONGRepository : EntidadeBaseRepository<ONG>, IONGRepository
    {
        public ONGRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}