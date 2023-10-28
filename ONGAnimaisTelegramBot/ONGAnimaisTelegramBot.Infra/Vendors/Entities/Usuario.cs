using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities
{
    public class Usuario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("telegramId")]
        public string TelegramId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("telefone")]
        public Telefone Telefone { get; set; } = new Telefone();

        [JsonPropertyName("endereco")]
        public Endereco Endereco { get; set; } = new Endereco();

        [JsonPropertyName("eventosSeguidos")]
        public List<Evento> EventosSeguidos { get; set; } = new List<Evento>();

        [JsonPropertyName("onGsSeguidas")]
        public List<ONG> ONGsSeguidas { get; set; } = new List<ONG> { };

        [JsonPropertyName("geoLocalizacao")]
        public Geolocalizacao Geolocalizacao { get; set; } = new Geolocalizacao();
    }
}