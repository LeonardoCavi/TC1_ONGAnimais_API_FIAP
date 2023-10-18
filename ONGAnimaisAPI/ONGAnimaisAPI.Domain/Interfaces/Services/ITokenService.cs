namespace ONGAnimaisAPI.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        object GerarToken(string usuario);
    }
}