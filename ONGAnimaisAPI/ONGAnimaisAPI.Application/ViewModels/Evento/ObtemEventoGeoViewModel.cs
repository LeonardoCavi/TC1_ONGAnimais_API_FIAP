using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Application.ViewModels.Evento
{
    public class ObtemEventoGeoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public GeoLocalizacao GeoLocalizacao { get; set; }
        public int OngId { get; set; }
    }
}