using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Application.ViewModels.ONG
{
    public class ObtemONGGeoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public GeoLocalizacao GeoLocalizacao { get; set; }
    }
}