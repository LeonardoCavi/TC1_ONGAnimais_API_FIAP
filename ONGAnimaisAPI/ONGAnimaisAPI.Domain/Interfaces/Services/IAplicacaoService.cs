namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface IAplicacaoService
    {
        Task<string> Autenticar(string usuario, string senha);
    }
}