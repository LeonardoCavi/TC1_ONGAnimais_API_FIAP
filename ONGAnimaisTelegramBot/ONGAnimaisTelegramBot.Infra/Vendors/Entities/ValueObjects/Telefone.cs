using ONGAnimaisTelegramBot.Infra.Vendors.Entities.Enums;
using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects
{
    public class Telefone
    {
        [JsonPropertyName("ddd")]
        public string DDD { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("tipo")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTelefone Tipo { get; set; }
    }
}