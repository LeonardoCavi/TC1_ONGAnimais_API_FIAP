using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Domain.Interfaces.Repositories
{
    public interface IAplicacaoRepository
    {
        Task<Aplicacao> Autenticar(string usuario, string senha);
    }
}