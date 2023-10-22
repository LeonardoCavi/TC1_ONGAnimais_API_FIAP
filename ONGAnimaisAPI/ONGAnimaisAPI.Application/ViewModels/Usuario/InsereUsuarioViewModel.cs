using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Application.ViewModels.Usuario
{
    public class InsereUsuarioViewModel
    {
        public string Nome { get; set; }
        public string TelegramId { get; set; }
        public Telefone Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public GeoLocalizacao GeoLocalizacao { get; set; }
    }
}