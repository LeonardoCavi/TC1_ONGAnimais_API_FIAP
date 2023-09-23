using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface IONGService
    {
        #region ONG
        Task InserirONG(ONG ong);

        Task<ONG> ObterONG(int id);

        Task<ONG> ObterONGEventos(int id);

        Task<ICollection<ONG>> ObterTodasONG();

        Task AtualizarONG(ONG ong);

        Task ExcluirONG(int id);

        #endregion

        #region Evento
        Task InserirEvento(Evento evento);

        Task<Evento> ObterEvento(int ongId, int id);

        Task<ICollection<Evento>> ObterTodosEventos();

        Task AtualizarEvento(Evento evento);

        Task ExcluirEvento(int ongId, int id);
        #endregion
    }
}