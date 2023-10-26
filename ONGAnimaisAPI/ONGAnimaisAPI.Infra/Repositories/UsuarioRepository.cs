using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repository;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class UsuarioRepository : EntidadeBaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        #region [Usuario]

        public async Task<Usuario> ObterUsuarioEventos(int id)
        {
            return await _dBSet
                .Include(u => u.EventosSeguidos.Where(e => e.Data >= DateTime.Now))
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> ObterUsuarioONGs(int id)
        {
           return await _dBSet.Include(u => u.ONGsSeguidas)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> ObterUsuarioPorTelegramId(string telegramId)
        {
            return await _dBSet
                .Include(u => u.ONGsSeguidas)
                .Include(u => u.EventosSeguidos
                    .Where(e => e.Data >= DateTime.Now))
                .FirstOrDefaultAsync(u => u.TelegramId == telegramId);
        }

        #endregion

    }
}