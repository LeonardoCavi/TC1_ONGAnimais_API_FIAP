using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class EntidadeBaseRepository<TEntidade> : IEntidadeBaseRepository<TEntidade> where TEntidade : EntidadeBase
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntidade> _dBSet;

        public EntidadeBaseRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._dBSet = _context.Set<TEntidade>();
        }

        public async Task Atualizar(TEntidade entidade)
        {
            _context.Update(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            _context.Remove(await Obter(id));
            await _context.SaveChangesAsync();
        }

        public async Task Inserir(TEntidade entidade)
        {
            await _context.AddAsync(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntidade> Obter(int id)
        {
            return await _dBSet.FindAsync(id);
        }

        public async Task<ICollection<TEntidade>> ObterTodos()
        {
            return await _dBSet.ToListAsync();
        }
    }
}