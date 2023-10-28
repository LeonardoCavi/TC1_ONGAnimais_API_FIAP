using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Application.ViewModels.Usuario
{
    public class ObtemUsuarioTelegramViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TelegramId { get; set; }
        public Telefone Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public GeoLocalizacao GeoLocalizacao { get; set; }
        public List<ObtemONGViewModel> ONGsSeguidas { get; set; }
        public List<ObtemEventoViewModel> EventosSeguidos { get; set; }
    }
}