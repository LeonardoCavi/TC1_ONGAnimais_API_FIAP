using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities
{
    public class Usuario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("telefone")]
        public Telefone Telefone { get; set; }

        [JsonPropertyName("endereco")]
        public Endereco Endereco { get; set; }

        [JsonPropertyName("eventosSeguidos")]
        public List<Evento> EventosSeguidos { get; set; }

        [JsonPropertyName("onGsSeguidas")]
        public List<ONG> ONGsSeguidas { get; set; }
    }
}