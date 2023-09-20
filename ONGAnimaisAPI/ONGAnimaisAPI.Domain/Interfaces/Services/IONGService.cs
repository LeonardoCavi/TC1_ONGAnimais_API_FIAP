using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface IONGService
    {
        Task InserirONG(ONG ong);

        Task<ONG> ObterONG(int id);

        Task<ICollection<ONG>> ObterTodasONG();

        Task AtualizarONG(ONG ong);

        Task ExcluirONG(int id);
    }
}