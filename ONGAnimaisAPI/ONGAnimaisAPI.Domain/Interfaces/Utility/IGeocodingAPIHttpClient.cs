using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Domain.Interfaces.Utility
{
    public interface IGeocodingAPIHttpClient
    {
        Task<GeoLocalizacao> BuscarLatLongPorEndereco(Endereco endereco);
    }
}