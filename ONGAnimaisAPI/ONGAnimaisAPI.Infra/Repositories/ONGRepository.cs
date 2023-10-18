using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repository;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class ONGRepository : EntidadeBaseRepository<ONG>, IONGRepository
    {
        public ONGRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ONG> ObterONGEventos(int id)
        {
            return await _dBSet.Include(o => o.Eventos)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<ICollection<ONG>> ObterONGsPorCidade(string cidade, string uf, int paginacao = 0)
        {
            return await _dBSet
                .Where(o => o.Endereco.Cidade == cidade &&
            o.Endereco.UF == uf)
                .Skip(paginacao * 5)
                .Take(5)
                .ToListAsync();
        }
    }
}