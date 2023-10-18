using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects
{
    public class Contato
    {
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; }
    }
}