using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities
{
    public class ONG
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("responsavel")]
        public string Responsavel { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("endereco")]
        public Endereco Endereco { get; set; }

        [JsonPropertyName("geoLocalizacao")]
        public Geolocalizacao Geolocalizacao { get; set; }

        [JsonPropertyName("telefones")]
        public List<Telefone> Telefones { get; set; }

        [JsonPropertyName("contatos")]
        public List<Contato> Contatos { get; set; }

        [JsonPropertyName("eventos")]
        public List<Evento> Eventos { get; set; }
    }
}