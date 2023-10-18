using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repositories;
using ONGAnimaisAPI.Domain.Interfaces.Repository;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class AplicacaoRepository : IAplicacaoRepository
    {
        protected readonly ApplicationDbContext _context;

        public AplicacaoRepository(ApplicationDbContext context)
        {
            this._context= context;
        }

        public async Task<Aplicacao> Autenticar(string usuario, string senha)
        {
            return await _context.Aplicacoes
                .Where(a => a.Usuario == usuario && a.Senha == senha)
                .FirstOrDefaultAsync();
        }
    }
}