using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities
{
    public class Evento
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("endereco")]
        public Endereco Endereco { get; set; }

        [JsonPropertyName("geolocalizacao")]
        public Geolocalizacao Geolocalizacao { get; set; }

        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("ongId")]
        public int OngId { get; set; }
    }
}