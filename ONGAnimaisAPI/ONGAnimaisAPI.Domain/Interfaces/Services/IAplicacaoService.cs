namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface IAplicacaoService
    {
        Task<object> Autenticar(string usuario, string senha);
    }
}