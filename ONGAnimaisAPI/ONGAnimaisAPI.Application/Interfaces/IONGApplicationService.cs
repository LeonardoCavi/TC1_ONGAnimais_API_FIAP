using ONGAnimaisAPI.Application.ViewModels.ONG;

namespace ONGAnimaisAPI.Application.Interfaces
{
    public interface IONGApplicationService
    {
        Task InserirONG(InsereONGViewModel ong);

        Task<ObtemONGViewModel> ObterONG(int id);

        Task<ICollection<ObtemONGViewModel>> ObterTodasONG();

        Task AtualizarONG(AtualizaONGViewModel ong);

        Task ExcluirONG(int id);
    }
}