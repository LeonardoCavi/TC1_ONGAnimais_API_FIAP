using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Application.Interfaces
{
    public interface IONGApplicationService
    {
        #region [ONG]
        Task InserirONG(InsereONGViewModel ong);

        Task<ObtemONGViewModel> ObterONG(int id);

        Task<ICollection<ObtemONGViewModel>> ObterTodasONG();

        Task<ICollection<ObtemONGViewModel>> ObterONGsPorCidade(BuscaONGCidadeViewModel ongcidade, int paginacao = 0);

        Task<ICollection<ObtemONGGeoViewModel>> ObterONGsPorCidadeGeo(decimal latitude, decimal longitude, int paginacao = 0);

        Task<ObtemONGEventosViewModel> ObterONGEventos(int id);

        Task AtualizarONG(AtualizaONGViewModel ong);

        Task ExcluirONG(int id);

        #endregion

        #region [Evento]

        Task InserirEvento(int ongId, InsereEventoViewModel evento);

        Task AtualizarEvento(int ongId, AtualizaEventoViewModel evento);

        Task<ObtemEventoViewModel> ObterEvento(int ongId, int id);

        Task ExcluirEvento(int ongId, int id);

        Task<ICollection<ObtemEventoViewModel>> ObterEventosPorCidade(BuscaEventoCidadeViewModel eventocidade, int paginacao = 0);

        Task<ICollection<ObtemEventoGeoViewModel>> ObterEventosPorCidadeGeo(decimal latitude, decimal longitude, int paginacao = 0);

        #endregion 
    }
}